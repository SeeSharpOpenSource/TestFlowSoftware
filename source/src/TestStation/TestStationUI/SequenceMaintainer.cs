using System;
using System.Collections.Generic;
using System.Linq;
using Testflow.Data;
using Testflow.Data.Description;
using Testflow.Data.Sequence;
using Testflow.Modules;
using Testflow.Runtime;
using Testflow.Utility.Utils;
using TestStation.Common;

namespace TestStation
{
    internal class SequenceMaintainer
    {
        private readonly GlobalInfo _globalInfo;
        private readonly ISequenceGroup _sequenceData;
        private Dictionary<string, ISequenceStep> _stepMapping;
        private ISequenceGroup _runtimeSequence;

        public bool UserEndTiming { get; private set; }
        public bool UserStartTiming { get; private set; }

        public string UutStartStack { get; private set; }
        public string UutOverStack { get; private set; }
        public string MainStartStack { get; private set; }
        public string MainOverStack { get; private set; }

        public SequenceMaintainer(ISequenceGroup sequenceData, GlobalInfo globalInfo)
        {
            _globalInfo = globalInfo;
            this._sequenceData = sequenceData;
            _stepMapping = new Dictionary<string, ISequenceStep>(500);
            _runtimeSequence = null;
            _globalInfo.BreakIfFailed = false;
        }

        const string CopyItemPostfix = "-Copy";

        public ISequenceGroup GetRuntimeSequence()
        {
            _runtimeSequence = null;
            _sequenceData.Parameters = null;

            _globalInfo.TestflowEntity.SequenceManager.ValidateSequenceData(_sequenceData);
            // 修改所有step的tag为最上层的tag
            ModifySequenceTag(_sequenceData.SetUp);
            ModifySequenceTag(_sequenceData.TearDown);
            foreach (ISequence sequence in _sequenceData.Sequences)
            {
                ModifySequenceTag(sequence);
            }

            ISequenceGroup runtimeSequenceGroup = (ISequenceGroup)_sequenceData.Clone();
            ChangeCopySequenceName(runtimeSequenceGroup);
            ChangeCopyVariableName(runtimeSequenceGroup.Variables);
            _runtimeSequence = runtimeSequenceGroup;

            // Setup
            ModifySequence(runtimeSequenceGroup.SetUp, "ProcessSetup");

            // Cleanup
            ModifySequence(runtimeSequenceGroup.TearDown, "ProcessCleanup");

            // PreUUT
            ModifySequence(runtimeSequenceGroup.Sequences[0], "PreUUT");

            // MainSequence
            ModifySequence(runtimeSequenceGroup.Sequences[1], "MainSequence");

            // PostUUT
            ModifySequence(runtimeSequenceGroup.Sequences[2], "PostUUT");

            // 初始化Continue变量
            InitDefaultVariable();
            // 添加Continue变量控制是否继续执行的代码
            AddContinueControlStep(runtimeSequenceGroup);

            ISequenceStep uutStartStep = GetFirstFunctionStep(runtimeSequenceGroup.Sequences[0].Steps);
            // uutover应该用PostUUT开始执行前计算，因为MainSequence中的最后一个step可能执行不到
            ISequenceStep uutOverStep = GetLastFunctionStep(runtimeSequenceGroup.Sequences[0].Steps);
            ISequenceStep mainStartStep = GetFirstFunctionStep(runtimeSequenceGroup.Sequences[1].Steps);
            // mainover应该用PostUUt开始执行前计算，因为main函数最后的代码可能不会被执行
            ISequenceStep mainOverStep = GetFirstFunctionStep(runtimeSequenceGroup.Sequences[2].Steps);
            SetUserTimingFlagAndSetDefaultTime(runtimeSequenceGroup.Sequences[0],
                runtimeSequenceGroup.Sequences[1], runtimeSequenceGroup.Sequences[2]);

            // 更新序列的结构
            ChangeSequenceStructure(runtimeSequenceGroup);
            _globalInfo.TestflowEntity.SequenceManager.ValidateSequenceData(runtimeSequenceGroup);
            SetKeyStepProperties(uutStartStep, uutOverStep, mainStartStep, mainOverStep);
            UutStartStack = Utility.GetStepStack(0, uutStartStep);
            UutOverStack = Utility.GetStepStack(0, uutOverStep);
            MainStartStack = Utility.GetStepStack(0, mainStartStep);
            MainOverStack = Utility.GetStepStack(0, mainOverStep);

            // 更新Stack到Step的映射
            UpdateStackToSequenceMapping(runtimeSequenceGroup);

            return runtimeSequenceGroup;
        }

        public ISequenceStep GetStepByRawStack(string stack)
        {
            return _stepMapping.ContainsKey(stack) ? _stepMapping[stack] : null;
        }

        public ISequenceStep GetRuntimeStep(string stack)
        {
            return SequenceUtils.GetStepFromStack(_runtimeSequence, stack);
        }

        // 修改所有step的tag为最上层的tag
        private void ModifySequenceTag(ISequence sequence)
        {
            foreach (ISequenceStep sequenceStep in sequence.Steps)
            {
                string stack = GetStack(sequenceStep);
                SetStepTag(sequenceStep, stack);
            }
        }

        private void SetKeyStepProperties(params ISequenceStep[] steps)
        {
            foreach (ISequenceStep sequenceStep in steps)
            {
                sequenceStep.RecordStatus = true;
            }
        }


        private void UpdateStackToSequenceMapping(ISequenceGroup sequenceGroup)
        {
            _stepMapping.Clear();
            foreach (ISequenceStep sequenceStep in sequenceGroup.SetUp.Steps)
            {
                UpdateStackToSequenceMapping(sequenceStep);
            }

            foreach (ISequenceStep sequenceStep in sequenceGroup.TearDown.Steps)
            {
                UpdateStackToSequenceMapping(sequenceStep);
            }

            foreach (ISequence sequence in sequenceGroup.Sequences)
            {
                foreach (ISequenceStep sequenceStep in sequence.Steps)
                {
                    UpdateStackToSequenceMapping(sequenceStep);
                }
            }
        }

        private void UpdateStackToSequenceMapping(ISequenceStep step)
        {
            if (!string.IsNullOrWhiteSpace(step.Tag) && !Constants.ReservedStep.Equals(step.Tag))
            {
                ISequenceStep rawStep = GetStepByStack(step.Tag);
                string currentStack = GetStack(step);
                _stepMapping.Add(currentStack, rawStep);
            }
            if (null != step.SubSteps && step.SubSteps.Count > 0)
            {
                foreach (ISequenceStep subStep in step.SubSteps)
                {
                    UpdateStackToSequenceMapping(subStep);
                }
            }
        }

        private void SetStepTag(ISequenceStep step, string tag)
        {
            step.Tag = tag;
            if (null != step.SubSteps && step.SubSteps.Count > 0)
            {
                foreach (ISequenceStep subStep in step.SubSteps)
                {
                    SetStepTag(subStep, tag);
                }
            }
        }

        private string GetStack(ISequenceStep step)
        {
            ISequenceStep current = step;
            List<string> stack = new List<string>(3);
            stack.Insert(0, current.Index.ToString());
            // 插入StepIndex
            while (current.Parent is ISequenceStep)
            {
                current = (ISequenceStep)current.Parent;
                stack.Insert(0, current.Index.ToString());
            }
            // 写入sequenceIndex
            stack.Insert(0, ((ISequence)current.Parent).Index.ToString());
            // 写入SessionIndex
            stack.Insert(0, "0");
            return string.Join("_", stack);
        }

        private ISequenceStep GetStepByStack(string stack)
        {
            string[] stackElems = stack.Split('_');
            int sequenceIndex = int.Parse(stackElems[1]);
            ISequence sequence;
            if (sequenceIndex == -1)
            {
                sequence = _sequenceData.SetUp;
            }
            else if (sequenceIndex == -2)
            {
                sequence = _sequenceData.TearDown;
            }
            else
            {
                sequence = _sequenceData.Sequences[sequenceIndex];
            }
            ISequenceStep step = sequence.Steps[int.Parse(stackElems[2])];
            for (int i = 3; i < stackElems.Length; i++)
            {
                step = step.SubSteps[int.Parse(stackElems[i])];
            }
            return step;
        }

        private void ModifySequence(ISequence runtimeSequence, string parentName)
        {
            ChangeCopyVariableName(runtimeSequence.Variables);
            foreach (ISequenceStep step in runtimeSequence.Steps)
            {
                #region ChangeCopyStepName
                step.Name = step.Name.Remove(step.Name.IndexOf(CopyItemPostfix)).Trim();
                step.SubSteps[0].Name = step.SubSteps[0].Name.Remove(step.SubSteps[0].Name.IndexOf(CopyItemPostfix)).Trim();
                #endregion
                string[] str = step.SubSteps[0].Name.Split(':');         //判断Sequence Call：SequenceName

                #region Sequence Call
                if (str.Length > 1)
                {
                    ISequence copy_seq = _runtimeSequence.Sequences.FirstOrDefault(item => item.Name.Equals(str[1]));
                    step.SubSteps[0].SubSteps = copy_seq.Steps;
                    foreach (ISequenceStep sequenceCallStep in step.SubSteps[0].SubSteps)
                    {
                        SetStepProperties(sequenceCallStep);
                        if (sequenceCallStep.Name.Contains("-Copy"))
                        {
                            sequenceCallStep.Name = sequenceCallStep.Name.Remove(sequenceCallStep.Name.IndexOf("-Copy")).Trim();
                        }
                    }
                    #region sequence var => runtimeSequence var
                    foreach (IVariable variable in copy_seq.Variables)
                    {
                        runtimeSequence.Variables.Add(variable);
                    }
                    #endregion
                }
                #endregion
                else
                {
                    SetStepProperties(step);
                }

            }
        }

        private void SetUserTimingFlagAndSetDefaultTime(ISequence preUut, ISequence mainSequence, ISequence postUut)
        {
            IVariable timingEnableVar =
                _runtimeSequence.Variables.FirstOrDefault(item => item.Name.Equals(Constants.TimingEnableVar));
            IVariable startTimeVar =
                _runtimeSequence.Variables.FirstOrDefault(item => item.Name.Equals(Constants.StartTimeVar));
            IVariable endTimeVar =
                _runtimeSequence.Variables.FirstOrDefault(item => item.Name.Equals(Constants.EndTimeVar));
            if (null != startTimeVar)
            {
                startTimeVar.Value = DateTime.MinValue.ToString(Constants.TimeFormat);
                startTimeVar.ReportRecordLevel = RecordLevel.Trace;
            }
            if (null != endTimeVar)
            {
                endTimeVar.Value = DateTime.MinValue.ToString(Constants.TimeFormat);
                endTimeVar.ReportRecordLevel = RecordLevel.Trace;
            }
            if (null == timingEnableVar ||
                !timingEnableVar.Value.Equals(true.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                UserStartTiming = false;
                UserEndTiming = false;
                return;
            }
            UserStartTiming = null != startTimeVar && (HasTimingStepWithSpecifiedVariable(preUut.Steps, Constants.StartTimingMethod, startTimeVar.Name) ||
                              HasTimingStepWithSpecifiedVariable(mainSequence.Steps, Constants.StartTimingMethod, startTimeVar.Name));
            UserEndTiming = null != endTimeVar &&
                            HasTimingStepWithSpecifiedVariable(mainSequence.Steps, Constants.EndTimingMethod, endTimeVar.Name);
        }

        // 获取当前step或下级step是否包含指定类型参数的
        private bool HasTimingStepWithSpecifiedVariable(ISequenceStepCollection steps, string methodName, string variableName)
        {
            foreach (ISequenceStep step in steps)
            {
                IFunctionData stepFunction = step.Function;
                if (null != stepFunction && stepFunction.MethodName.Equals(methodName) && stepFunction.Return.Equals(variableName))
                {
                    return true;
                }
                if (step.HasSubSteps)
                {
                    if (HasTimingStepWithSpecifiedVariable(step.SubSteps, methodName, variableName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void AddContinueControlStep(ISequenceGroup runtimeSequence)
        {
            ISequenceManager sequenceManager = _globalInfo.TestflowEntity.SequenceManager;
            // 获取控制流程的Function描述信息
            IComInterfaceManager interfaceManager = _globalInfo.TestflowEntity.ComInterfaceManager;
            IComInterfaceDescription interfaceDescription = interfaceManager.GetComInterfaceByName("TestStationLimit");
            IClassInterfaceDescription loopClassDescription = interfaceDescription.Classes.First(item => item.Name.Equals("Loop"));
            IFuncInterfaceDescription isContinueFuncDesc = loopClassDescription.Functions.First(item => item.Name.Equals("IsContinue"));
            // 在PreUUT最后一部执行，判断Main是否执行的Step代码
            ISequenceStep preUutLastStep = sequenceManager.CreateSequenceStep(false);
            runtimeSequence.Sequences[0].Steps.Add(preUutLastStep);
            preUutLastStep.Function = sequenceManager.CreateFunctionData(isContinueFuncDesc);
            // 配置参数为Continue变量
            preUutLastStep.Function.Parameters[0].Value = Constants.ContinueVariable;
            preUutLastStep.Function.Parameters[0].ParameterType = ParameterType.Variable;
            preUutLastStep.AssertFailedAction = FailedAction.BreakLoop;
            preUutLastStep.InvokeErrorAction = FailedAction.BreakLoop;
            preUutLastStep.RecordStatus = true;

            // 在PostUUT第一步执行，判断Main是否执行的Step代码
            ISequenceStep postUutFirstStep = sequenceManager.CreateSequenceStep(false);
            runtimeSequence.Sequences[2].Steps.Insert(0, postUutFirstStep);
            postUutFirstStep.Function = sequenceManager.CreateFunctionData(isContinueFuncDesc);
            // 配置参数为Continue变量
            postUutFirstStep.Function.Parameters[0].Value = Constants.ContinueVariable;
            postUutFirstStep.Function.Parameters[0].ParameterType = ParameterType.Variable;
            postUutFirstStep.AssertFailedAction = FailedAction.BreakLoop;
            postUutFirstStep.InvokeErrorAction = FailedAction.BreakLoop;
            postUutFirstStep.RecordStatus = true;
        }

        private ISequenceStep GetFirstFunctionStep(ISequenceStepCollection steps)
        {
            foreach (ISequenceStep step in steps)
            {
                if (null != step.Function)
                {
                    return step;
                }
                if (step.HasSubSteps)
                {
                    ISequenceStep sequenceStep = GetFirstFunctionStep(step.SubSteps);
                    if (null != sequenceStep)
                    {
                        return sequenceStep;
                    }
                }
            }
            // 没有非空的Step，则添加一个空的Step作为标记
            steps.Clear();
            ISequenceStep emptyStep = _globalInfo.TestflowEntity.SequenceManager.CreateSequenceStep(false);
            emptyStep.Tag = Constants.ReservedStep;
            steps.Add(emptyStep);
            return emptyStep;
        }

        private ISequenceStep GetLastFunctionStep(ISequenceStepCollection steps)
        {
            for (int i = steps.Count - 1; i >= 0; i--)
            {
                ISequenceStep step = steps[i];
                if (null != step.Function)
                {
                    return step;
                }
                if (step.HasSubSteps)
                {
                    ISequenceStep sequenceStep = GetLastFunctionStep(step.SubSteps);
                    if (null != sequenceStep)
                    {
                        return sequenceStep;
                    }
                }
            }
            // 没有非空的Step，则添加一个空的Step作为标记
            steps.Clear();
            ISequenceStep emptyStep = _globalInfo.TestflowEntity.SequenceManager.CreateSequenceStep(false);
            emptyStep.Tag = Constants.ReservedStep;
            steps.Add(emptyStep);
            return emptyStep;
        }

        private void ChangeSequenceStructure(ISequenceGroup runtimeSequenceGroup)
        {
            ISequence preUutSequence = runtimeSequenceGroup.Sequences[0];
            ISequence testSequence = runtimeSequenceGroup.Sequences[1];
            ISequence finallySequence = runtimeSequenceGroup.Sequences[2];
            ISequenceManager sequenceManager = _globalInfo.TestflowEntity.SequenceManager;

            // 删除无用的序列
            runtimeSequenceGroup.Sequences.Clear();
            ISequence runSequence = sequenceManager.CreateSequence();
            runtimeSequenceGroup.Sequences.Add(runSequence);
            runSequence.Parent = runtimeSequenceGroup;

            foreach (IVariable variable in preUutSequence.Variables)
            {
                variable.Name = variable.Name.Replace("-Copy", "").Trim();
                runSequence.Variables.Add(variable);
            }
            foreach (IVariable variable in testSequence.Variables)
            {
                variable.Name = variable.Name.Replace("-Copy", "").Trim();
                runSequence.Variables.Add(variable);
            }
            foreach (IVariable variable in finallySequence.Variables)
            {
                runSequence.Variables.Add(variable);
            }

            sequenceManager.ValidateSequenceData(runtimeSequenceGroup);

            // 设置条件循环的loop为Continue变量控制
            IList<IFuncInterfaceDescription> funcList =
                _globalInfo.TestflowEntity.DesignTimeService.Components["TestStationLimit"].Classes.First(
                    item => item.Name.Equals("Loop")).Functions;
            ISequenceStep loopStep = sequenceManager.CreateNonExecutionStep(SequenceStepType.ConditionLoop);
            loopStep.AssertFailedAction = FailedAction.Terminate;
            loopStep.InvokeErrorAction = FailedAction.Terminate;
            loopStep.Function = _globalInfo.TestflowEntity.SequenceManager.CreateFunctionData(
                    funcList.FirstOrDefault(item => item.Name.Equals("SetBoolValue")));
            loopStep.Function.Parameters[0].Value = Constants.ContinueVariable;
            loopStep.Function.Parameters[0].ParameterType = ParameterType.Variable;
            // 添加UutIndex变量到循环控制中
            IVariable uutIndexVar = _globalInfo.TestflowEntity.SequenceManager.CreateVarialbe();
            uutIndexVar.Name = Constants.UutIndexVar;
            uutIndexVar.ReportRecordLevel = RecordLevel.FullTrace;
            uutIndexVar.Value = "-1";
            _runtimeSequence.Variables.Add(uutIndexVar);
            loopStep.LoopCounter = _globalInfo.TestflowEntity.SequenceManager.CreateLoopCounter();
            loopStep.LoopCounter.CounterEnabled = true;
            loopStep.LoopCounter.CounterVariable = Constants.UutIndexVar;

            ISequenceStep tryFinallyBlock = sequenceManager.CreateNonExecutionStep(SequenceStepType.TryFinallyBlock);

            runSequence.Steps.Add(loopStep);

            loopStep.SubSteps.Add(tryFinallyBlock);

            ISequenceStep tryBlock = tryFinallyBlock.SubSteps[0];
            ISequenceStep finallyBlock = tryFinallyBlock.SubSteps[1];

            // 移动PreUUT步骤到Try块代码下\
            foreach (ISequenceStep step in preUutSequence.Steps)
            {
                MoveStepsToParent(step, tryBlock, FailedAction.NextLoop);
            }
            // 移动MainSequence的steps到try块下
            foreach (ISequenceStep step in testSequence.Steps)
            {
                MoveStepsToParent(step, tryBlock, FailedAction.NextLoop);
            }
            // 移动PostUUT的代码到Finally块下
            foreach (ISequenceStep step in finallySequence.Steps)
            {
                MoveStepsToParent(step, finallyBlock, FailedAction.NextLoop);
            }

            sequenceManager.ValidateSequenceData(runtimeSequenceGroup);
        }

        private void MoveStepsToParent(ISequenceStep step, ISequenceStep parent, FailedAction defaultFailedAction)
        {
            // 如果上级节点的LoopCounter使能或RetryCounter使能或者该Stp是一个预留用以占位的Step，则将其添加到Try的Steps节点下
            if ((null != step.LoopCounter && step.LoopCounter.MaxValue > 1) || (null != step.RetryCounter) || step.Tag.Equals(Constants.ReservedStep))
            {
                parent.SubSteps.Add(step);
                FailedAction failedAction = step.BreakIfFailed ? FailedAction.Terminate : defaultFailedAction;
                step.AssertFailedAction = failedAction;
                step.InvokeErrorAction = FailedAction.BreakLoop;
                // 如果当前step包含loopCounter或RetryCounter，则需要将下级的SubSteps全部添加到当前step的子节点，否则可以添加到上级的子节点以降低栈调用
                parent = step;
            }
            else if (null != step.Function)
            {
                parent.SubSteps.Add(step);
            }
            ISequenceStepCollection subSteps = step.SubSteps;
            if (null != subSteps && subSteps.Count > 0)
            {
                List<ISequenceStep> sequenceSteps = new List<ISequenceStep>(subSteps.Count);
                // 循环内部会涉及到step的移动，需要拷贝后遍历
                // 原来的集合未实现CopyTo方法， 所以需要手动拷贝
                foreach (ISequenceStep sequenceStep in subSteps)
                {
                    sequenceSteps.Add(sequenceStep);
                }
                subSteps.Clear();
                foreach (ISequenceStep subStep in sequenceSteps)
                {
                    MoveStepsToParent(subStep, parent, defaultFailedAction);
                }
            }
        }

        private void InitDefaultVariable()
        {
            // 循环控制变量
            // 如果PreUUT里有Continue变量，则移动到SequenceGroup级
            IVariable continueVar;
            if (null != (continueVar = _runtimeSequence.Sequences[0].Variables.FirstOrDefault(item => item.Name.Equals(Constants.ContinueVariable))))
            {
                _runtimeSequence.Sequences[0].Variables.Remove(continueVar);
                if (_runtimeSequence.Variables.Any(item => item.Name.Equals(Constants.ContinueVariable)))
                {
                    IVariable existVar = _runtimeSequence.Variables.First(item => item.Name.Equals(Constants.ContinueVariable));
                    _runtimeSequence.Variables.Remove(existVar);
                }
                _runtimeSequence.Variables.Add(continueVar);
            }
            // 如果不存在则直接创建
            else if (!_runtimeSequence.Variables.Any(item => item.Name.Equals(Constants.ContinueVariable)))
            {
                Type boolType = typeof(bool);
                continueVar = _globalInfo.TestflowEntity.SequenceManager.CreateVarialbe();
                continueVar.Name = Constants.ContinueVariable;
                continueVar.Value = "True";
                _runtimeSequence.Variables.Add(continueVar);
                continueVar.Type = _globalInfo.TestflowEntity.ComInterfaceManager.GetTypeByName(boolType.Name,
                    boolType.Namespace);
                continueVar.ReportRecordLevel = RecordLevel.None;
            }
        }

        private void ChangeCopySequenceName(ISequenceGroup sequenceData)
        {
            if (sequenceData.SetUp.Name.Contains(CopyItemPostfix))
            {
                sequenceData.SetUp.Name = sequenceData.SetUp.Name.Replace(CopyItemPostfix, "").Trim();
            }
            if (sequenceData.TearDown.Name.Contains(CopyItemPostfix))
            {
                sequenceData.TearDown.Name = sequenceData.TearDown.Name.Replace(CopyItemPostfix, "").Trim();
            }
            foreach (ISequence sequence in sequenceData.Sequences)
            {
                if (sequence.Name.Contains(CopyItemPostfix))
                {
                    sequence.Name = sequence.Name.Replace(CopyItemPostfix, "").Trim();
                }
            }
        }

        private void ChangeCopyVariableName(IVariableCollection variables)
        {
            foreach (IVariable variable in variables)
            {
                if (variable.Name.Contains(CopyItemPostfix))
                {
                    variable.Name = variable.Name.Remove(variable.Name.IndexOf(CopyItemPostfix)).Trim();
                }
            }
        }

        private void SetStepProperties(ISequenceStep rootStep)
        {
            bool userAssignAction = rootStep.BreakIfFailed;
            rootStep.AssertFailedAction = userAssignAction ? FailedAction.NextLoop : FailedAction.Continue;
            rootStep.InvokeErrorAction = rootStep.AssertFailedAction; ;
            foreach (ISequenceStep step in rootStep.SubSteps)
            {
                step.InvokeErrorAction = rootStep.InvokeErrorAction;
                step.AssertFailedAction = rootStep.AssertFailedAction;
                step.RecordStatus = rootStep.RecordStatus;
            }
        }

        public string GetSequencePath()
        {
            return _sequenceData.Info.SequenceGroupFile;
        }
    }
}