using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Testflow;
using Testflow.Data;
using Testflow.Data.Description;
using Testflow.Data.Sequence;
using Testflow.Modules;
using Testflow.Runtime;
using Testflow.Utility.Utils;
using TestFlow.SoftDSevCommon;

namespace TestFlow.SoftDevCommon
{
    public class SequenceMaintainer
    {
        private readonly GlobalInfo _globalInfo;
        private readonly ISequenceGroup _sequenceData;
        private Dictionary<string, ISequenceStep> _stepMapping;
        private ISequenceGroup _runtimeSequence;
        // 表达式匹配模式，第1组为数组的源数据，第二组为数组的变量名称
        private readonly Regex _expRegex;
        private readonly Regex _varRegex;
        // 变量原始名称到变量运行时名称的映射
        private readonly Dictionary<int, Dictionary<string, string>> _varNameMapping;
        // 表达式替代变量到表达式的映射
        private readonly Dictionary<int, Dictionary<string, string>> _expVarMapping; 

        public bool UserEndTiming { get; private set; }
        public bool UserStartTiming { get; private set; }

        public string UutStartStack { get; private set; }
        public string UutOverStack { get; private set; }
        public string MainStartStack { get; private set; }
        public string MainOverStack { get; private set; }
        public string PostStartStack { get; private set; }

        public SequenceMaintainer(ISequenceGroup sequenceData, GlobalInfo globalInfo)
        {
            _globalInfo = globalInfo;
            this._sequenceData = sequenceData;
            _stepMapping = new Dictionary<string, ISequenceStep>(500);
            _runtimeSequence = null;
            _globalInfo.BreakIfFailed = false;
            this._expRegex = new Regex("^(([^\\.]+)(?:\\.[^\\.]+)*)\\[(\\d+)\\]$", RegexOptions.Compiled);
            this._varRegex = new Regex("^(?:\\d+\\$)*(([^\\$]+)_\\d+)$", RegexOptions.Compiled);
            _varNameMapping = new Dictionary<int, Dictionary<string, string>>(10);
            _expVarMapping = new Dictionary<int, Dictionary<string, string>>(10);
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
            runtimeSequenceGroup.Name = _sequenceData.Name;
            ChangeCopySequenceGroupName(runtimeSequenceGroup);
            _runtimeSequence = runtimeSequenceGroup;
            // 初始化Continue变量
            InitDefaultVariable();
            _globalInfo.TestflowEntity.SequenceManager.ValidateSequenceData(_runtimeSequence);
            // 处理序列中配置的Condition
            ProcessConditionStep(runtimeSequenceGroup);
            // 重命名所有变量为运行时名称，即varName_seqName
            RenameVariableToRuntimeName(runtimeSequenceGroup);
            // 预处理序列，内联SequenceCall的可执行，修改变量为运行时名称，处理表达式参数
            PreProcessSequenceData(runtimeSequenceGroup);
            
            // 添加Continue变量控制是否继续执行的代码
            AddContinueControlStep(runtimeSequenceGroup);

            ISequenceStep uutStartStep = GetFirstFunctionStep(runtimeSequenceGroup.Sequences[0].Steps, runtimeSequenceGroup.Sequences[0]);
            // uutover应该用PostUUT开始执行前计算，因为MainSequence中的最后一个step可能执行不到
            ISequenceStep uutOverStep = GetLastFunctionStep(runtimeSequenceGroup.Sequences[0].Steps, runtimeSequenceGroup.Sequences[0]);
            ISequenceStep mainStartStep = GetFirstFunctionStep(runtimeSequenceGroup.Sequences[1].Steps, runtimeSequenceGroup.Sequences[1]);
            ISequenceStep mainOverStep = GetLastFunctionStep(runtimeSequenceGroup.Sequences[1].Steps, runtimeSequenceGroup.Sequences[1]);
            // mainover应该用PostUUt开始执行前计算，因为main函数最后的代码可能不会被执行
            ISequenceStep postStartStep = GetFirstFunctionStep(runtimeSequenceGroup.Sequences[2].Steps, runtimeSequenceGroup.Sequences[2]);
            SetUserTimingFlagAndSetDefaultTime(runtimeSequenceGroup.Sequences[0],
                runtimeSequenceGroup.Sequences[1], runtimeSequenceGroup.Sequences[2]);

            // 更新序列的结构
            ChangeSequenceStructure(runtimeSequenceGroup);
            _globalInfo.TestflowEntity.SequenceManager.ValidateSequenceData(runtimeSequenceGroup);
            SetKeyStepProperties(uutStartStep, uutOverStep, mainStartStep, postStartStep);
            UutStartStack = Utility.GetStepStack(0, uutStartStep);
            UutOverStack = Utility.GetStepStack(0, uutOverStep);
            MainStartStack = Utility.GetStepStack(0, mainStartStep);
            MainOverStack = Utility.GetStepStack(0, mainOverStep);
            PostStartStack = Utility.GetStepStack(0, postStartStep);

            // 更新Stack到Step的映射
            UpdateStackToSequenceMapping(runtimeSequenceGroup);

            return runtimeSequenceGroup;
        }

        private void RenameVariableToRuntimeName(ISequenceGroup runtimeSequenceGroup)
        {
            _varNameMapping.Clear();
            Dictionary<string, string> globalNameMapping = RenameGlobalVariables(runtimeSequenceGroup.Variables);
            _varNameMapping.Add(Constants.SequenceGroupIndex, globalNameMapping);
            Dictionary<string, string> setUpVarMapping = RenameVariables(runtimeSequenceGroup.SetUp.Variables);
            _varNameMapping.Add(runtimeSequenceGroup.SetUp.Index, setUpVarMapping);
            Dictionary<string, string> tearDownVarMapping = RenameVariables(runtimeSequenceGroup.TearDown.Variables);
            _varNameMapping.Add(runtimeSequenceGroup.TearDown.Index, tearDownVarMapping);
            foreach (ISequence sequence in runtimeSequenceGroup.Sequences)
            {
                Dictionary<string, string> seqVarMapping = RenameVariables(sequence.Variables);
                _varNameMapping.Add(sequence.Index, seqVarMapping);
            }
        }

        private void PreProcessSequenceData(ISequenceGroup runtimeSequenceGroup)
        {
            _expVarMapping.Clear();

            for (int i = 3; i < runtimeSequenceGroup.Sequences.Count; i++)
            {
                ModifySequence(runtimeSequenceGroup.Sequences[i], runtimeSequenceGroup.Sequences[i].Name);
            }

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
        }

        public ISequenceStep GetStepByStack(ICallStack stack)
        {
            int sequenceIndex = stack.Sequence;
            ISequence sequence;
            if (sequenceIndex == -1)
            {
                sequence = _runtimeSequence.SetUp;
            }
            else if (sequenceIndex == -2)
            {
                sequence = _runtimeSequence.TearDown;
            }
            else
            {
                sequence = _runtimeSequence.Sequences[sequenceIndex];
            }
            ISequenceStep step = sequence.Steps[stack.StepStack[0]];
            for (int i = 1; i < stack.StepStack.Count; i++)
            {
                step = step.SubSteps[stack.StepStack[i]];
            }
            return step;
        }

        public ISequenceStep GetStepByRawStack(string stack)
        {
            return _stepMapping.ContainsKey(stack) ? _stepMapping[stack] : null;
        }

        public string GetRuntimeVariable(int sequenceIndex, string variable)
        {
            if (_varNameMapping[sequenceIndex].ContainsKey(variable))
            {
                return _varNameMapping[sequenceIndex][variable];
            }
            else if (_varNameMapping[Constants.SequenceGroupIndex].ContainsKey(variable))
            {
                return _varNameMapping[Constants.SequenceGroupIndex][variable];
            }
            return null;
        }

        public IEnumerable<string> GetExpVariables(int seqIndex)
        {
            return _expVarMapping[seqIndex].Keys;
        }

        public string GetRawVariable(int seqIndex, string runtimeVariable)
        {
            Match matchData = _varRegex.Match(runtimeVariable);
            if (!matchData.Success)
            {
                return runtimeVariable;
            }
            // 如果是表达式数据，则返回表达式
            if (_expVarMapping[seqIndex].ContainsKey(matchData.Groups[1].Value))
            {
                return _expVarMapping[seqIndex][matchData.Groups[1].Value];
            }
            return matchData.Groups[2].Value;
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

        private void ProcessConditionStep(ISequenceGroup sequenceGroup)
        {
            ProcessConditionStep(sequenceGroup.SetUp);
            ProcessConditionStep(sequenceGroup.TearDown);
            foreach (ISequence sequence in sequenceGroup.Sequences)
            {
                ProcessConditionStep(sequence);
            }
        }

        private void ProcessConditionStep(ISequence sequence)
        {
            foreach (ISequenceStep sequenceStep in sequence.Steps)
            {
                ProcessConditionStep(sequenceStep);
            }
        }

        private void ProcessConditionStep(ISequenceStep step)
        {
            ISequenceStep conditionStep =
                step.SubSteps.FirstOrDefault(item => item.Name.Equals(Constants.ConditionStepName));
            // 为当前步骤新增Condition判断Function
            if (null == conditionStep)
            {
                return;
            }
            step.SubSteps.Remove(conditionStep);
            string assertType, conditionVar;
            Utility.GetConditionProperty(conditionStep.Description, out assertType, out conditionVar);
            if (string.IsNullOrWhiteSpace(conditionVar))
            {
                throw new ApplicationException($"The condition variable of step '{step.Name}' has not been configured.");
            }
            TestflowRunner testflowEntity = _globalInfo.TestflowEntity;
            ITypeData loopType = testflowEntity.ComInterfaceManager.GetTypeByName("Loop", "TestStationLimit");
            IAssemblyInfo assemblyInfo;
            IClassInterfaceDescription classDescription = testflowEntity.ComInterfaceManager.
                GetClassDescriptionByType(loopType, out assemblyInfo);
            IFuncInterfaceDescription funcDescription = null;
            switch (assertType)
            {
                case Constants.ConditionTrue:
                    funcDescription = classDescription.Functions.First(item => item.Name.Equals("IsTrue"));
                    break;
                case Constants.ConditionFalse:
                    funcDescription = classDescription.Functions.First(item => item.Name.Equals("IsFalse"));
                    break;
                default:
                    return;
                    break;
            }
            IFunctionData functionData = testflowEntity.SequenceManager.CreateFunctionData(funcDescription);
            functionData.Parameters[0].ParameterType = ParameterType.Variable;
            functionData.Parameters[0].Value = conditionVar;
            step.Function = functionData;
            step.StepType = SequenceStepType.ConditionStatement;
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
            foreach (ISequenceStep step in runtimeSequence.Steps)
            {
                string[] str = step.SubSteps[0].Name.Split(':');         //判断Sequence Call：SequenceName

                #region Sequence Call
                if (str.Length > 1)
                {
                    ISequence copySeq = _runtimeSequence.Sequences.FirstOrDefault(item => item.Name.Equals(str[1]));
                    if (null == copySeq)
                    {
                        throw new ApplicationException($"Subsequence <{str[1]}> does not exist.");
                    }
                    AddSubSequenceSteps(runtimeSequence, step.SubSteps, copySeq);
                }
                #endregion
                else
                {
                    SetStepProperties(step);
                    ISequenceStep constructorStep = step.SubSteps.FirstOrDefault(
                        item => null != item.Function && item.Function.Type == FunctionType.Constructor);
                    ISequenceStep functionStep = step.SubSteps.FirstOrDefault(
                        item => item.Name.Equals(Constants.MethodStepName));
                    if (null != constructorStep && functionStep?.Function != null &&
                        string.IsNullOrWhiteSpace(functionStep.Function.Instance))
                    {
                        AddDefaultInstanceVariable(runtimeSequence, constructorStep, functionStep);
                    }
                    // Constructor不应该打印信息
                    if (constructorStep != null)
                    {
                        constructorStep.RecordStatus = false;
                    }
                }
            }
            IComInterfaceDescription interfaceDescription =
                _globalInfo.TestflowEntity.ComInterfaceManager.GetComInterfaceByName("TestStationLimit");
            IClassInterfaceDescription classDescription = interfaceDescription.Classes.First(item => item.Name.Equals("Expression"));
            IFuncInterfaceDescription funcDescription = classDescription.Functions.First(item => item.Name.Equals("GetArrayElement"));
            // 更新序列中每个参数中变量的名称为运行时变量名称
            Dictionary<string, string> seqExpVarMapping = new Dictionary<string, string>(10);
            _expVarMapping.Add(runtimeSequence.Index, seqExpVarMapping);
            foreach (ISequenceStep sequenceStep in runtimeSequence.Steps)
            {
                UpdateRuntimeVarNameToParameter(sequenceStep, runtimeSequence.Index);
                ProcessExpression(runtimeSequence, sequenceStep, funcDescription, seqExpVarMapping);
            }
        }

        private Dictionary<string, string> RenameGlobalVariables(IVariableCollection variables)
        {
            Dictionary<string, string> nameMapping = new Dictionary<string, string>(variables.Count);
            if (variables.Count <= 0)
            {
                return nameMapping;
            }
            ISequenceFlowContainer parent = variables[0].Parent;
            string variablePostfix = parent is ISequenceGroup
                ? $"_{Constants.SequenceGroupIndex}"
                : $"_{((ISequence)parent).Index}";
            foreach (IVariable variable in variables)
            {
                // 跳过系统变量
                if (variable.Name.Equals(Constants.ContinueVariable) || variable.Name.Equals(Constants.TimingEnableVar) ||
                    variable.Name.Equals(Constants.StartTimeVar) || variable.Name.Equals(Constants.EndTimeVar) ||
                    variable.Name.Equals(Constants.SerialNoVarName) || variable.Name.Equals(Constants.DutIndexVarName) ||
                    variable.Name.Equals(Constants.UutIndexVar))
                {
                    nameMapping.Add(variable.Name, variable.Name);
                    continue;
                }
                string newVariableName = variable.Name + variablePostfix;
                nameMapping.Add(variable.Name, newVariableName);
                variable.Name = newVariableName;
            }
            return nameMapping;
        }

        private Dictionary<string, string> RenameVariables(IVariableCollection variables)
        {
            Dictionary<string, string> nameMapping = new Dictionary<string, string>(variables.Count);
            if (variables.Count <= 0)
            {
                return nameMapping;
            }
            ISequenceFlowContainer parent = variables[0].Parent;
            string variablePostfix = parent is ISequenceGroup
                ? $"_{Constants.SequenceGroupIndex}"
                : $"_{((ISequence) parent).Index}";
            foreach (IVariable variable in variables)
            {
                string newVariableName = variable.Name + variablePostfix;
                nameMapping.Add(variable.Name, newVariableName);
                variable.Name = newVariableName;
            }
            return nameMapping;
        }

        private void UpdateRuntimeVarNameToParameter(ISequenceStep step, int sequenceIndex)
        {
            Dictionary<string, string> globalVarMapping = _varNameMapping[Constants.SequenceGroupIndex];
            Dictionary<string, string> localVarMapping = _varNameMapping[sequenceIndex];
            if (!step.HasSubSteps)
            {
                return;
            }
            foreach (ISequenceStep subStep in step.SubSteps)
            {
                // FunctionStep的名称
                if (null != subStep.Function)
                {
                    if (!string.IsNullOrWhiteSpace(subStep.Function.Instance))
                    {
                        subStep.Function.Instance = GetNewParameterValue(ParameterType.Variable,
                            subStep.Function.Instance, localVarMapping, globalVarMapping);
                    }
                    if (!string.IsNullOrWhiteSpace(subStep.Function.Return))
                    {
                        subStep.Function.Return = GetNewParameterValue(ParameterType.Variable, subStep.Function.Return,
                            localVarMapping, globalVarMapping);
                    }
                    if (null != subStep.Function.Parameters)
                    {
                        foreach (IParameterData parameterData in subStep.Function.Parameters)
                        {
                            ParameterType parameterType = parameterData.ParameterType;
                            parameterData.Value = GetNewParameterValue(parameterType, parameterData.Value, localVarMapping,
                                globalVarMapping);
                        }
                    }
                }
                // 修改Condition中变量的名称
                else if (subStep.Name.Equals(Constants.ConditionStepName))
                {
                    string[] conditionElem = subStep.Description.Split('%');
                    if (conditionElem.Length != 2 || string.IsNullOrWhiteSpace(conditionElem[1]))
                    {
                        return;
                    }
                    string value = conditionElem[1];
                    value = GetNewParameterValue(ParameterType.Variable, value, localVarMapping, globalVarMapping);
                    subStep.Description = $"{conditionElem[0]}%{value}";
                }
            }
        }

        private string GetNewParameterValue(ParameterType parameterType, string value, Dictionary<string, string> localVarMapping, Dictionary<string, string> globalVarMapping)
        {
            switch (parameterType)
            {
                case ParameterType.NotAvailable:
                case ParameterType.Value:
                    break;
                case ParameterType.Variable:
                    if (localVarMapping.ContainsKey(value))
                    {
                        value = localVarMapping[value];
                    }
                    else if (globalVarMapping.ContainsKey(value))
                    {
                        value = globalVarMapping[value];
                    }
//                    else
//                    {
//                        throw new ApplicationException($"Variable {value} does not exist.");
//                    }
                    break;
                case ParameterType.Expression:
                    Match match = _expRegex.Match(value);
                    if (!match.Success)
                    {
                        throw new ApplicationException($"Invalid expression: {value}.");
                    }
                    // 表达式的不做处理，在ProcessExpression中再执行
//                    string varName = match.Groups[2].Value;
//                    string newVarName;
//                    if (localVarMapping.ContainsKey(varName))
//                    {
//                        newVarName = localVarMapping[varName];
//                    }
//                    else if (globalVarMapping.ContainsKey(varName))
//                    {
//                        newVarName = globalVarMapping[varName];
//                    }
//                    else
//                    {
//                        throw new ApplicationException($"Variable {value} does not exist.");
//                    }
//                    // TODO 此处在变量内部存在相同字符串时出现问题，后续再解决
//                    value = value.Replace(varName, newVarName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return value;
        }

        private void ProcessExpression(ISequence parent, ISequenceStep step, IFuncInterfaceDescription funcDescription,
            Dictionary<string, string> seqExpMapping)
        {
            if (!step.HasSubSteps)
            {
                return;
            }
            Dictionary<string, string> localVarMapping = _varNameMapping[parent.Index];
            Dictionary<string, string> globalVarMapping = _varNameMapping[Constants.SequenceGroupIndex];
            ISequenceManager sequenceManager = _globalInfo.TestflowEntity.SequenceManager;
            int subStepCount = step.SubSteps.Count;
            for (int i = subStepCount - 1; i >= 0; i--)
            {
                IFunctionData functionData = step.SubSteps[i].Function;
                if (null == functionData?.Parameters)
                {
                    continue;
                }
                foreach (IParameterData parameterData in functionData.Parameters)
                {
                    if (parameterData.ParameterType != ParameterType.Expression)
                    {
                        continue;
                    }
                    Match matchData = _expRegex.Match(parameterData.Value);
                    string arrayVar = matchData.Groups[1].Value;
                    string newArrayVar = GetNewParameterValue(ParameterType.Variable, arrayVar, localVarMapping, globalVarMapping);
                    string index = matchData.Groups[3].Value;
                    // 获取记录表达式值的中间变量，并修改其名称，添加其值到参数名映射中
                    string expResultVarName;
                    // 如果已经存在计算该表达式的中间变量，则直接使用
                    if (localVarMapping.ContainsKey(parameterData.Value))
                    {
                        expResultVarName = localVarMapping[parameterData.Value];
                    }
                    else
                    {
                        // 如果不存在计算该表达式的中间变量，则创建变量
                        IVariable expResultVar = sequenceManager.CreateVariable();
                        expResultVar.ReportRecordLevel = RecordLevel.Trace;
                        parent.Variables.Add(expResultVar);
                        expResultVar.Initialize(parent);
                        expResultVarName = GetValidVariableName(parent, expResultVar);
                        expResultVar.Name = expResultVarName;
                        localVarMapping.Add(parameterData.Value, expResultVarName);
                        // 缓存变量名称到表达式的映射
                        seqExpMapping.Add(expResultVarName, parameterData.Value);
                    }
                    parameterData.Value = expResultVarName;
                    parameterData.ParameterType = ParameterType.Variable;

                    // 插入计算表达式值的step，配置对应参数
                    ISequenceStep expStep = sequenceManager.CreateSequenceStep(false);
                    expStep.Name = Constants.MethodStepName;
                    expStep.Tag = step.Tag;
                    expStep.AssertFailedAction = step.AssertFailedAction;
                    expStep.InvokeErrorAction = step.InvokeErrorAction;
                    expStep.Function = sequenceManager.CreateFunctionData(funcDescription);
                    expStep.Function.Parameters[0].Value = newArrayVar;
                    expStep.Function.Parameters[0].ParameterType = ParameterType.Variable;
                    expStep.Function.Parameters[1].Value = index;
                    expStep.Function.Parameters[1].ParameterType = ParameterType.Value;
                    expStep.Function.Return = expResultVarName;
                    step.SubSteps.Insert(i, expStep);
                }
            }
        }

        private static string GetValidVariableName(ISequence parent, IVariable expResultVar)
        {
            int index = 0;
            string varName;
            do
            {
                varName = $"{expResultVar.Name}_{index++}_{parent.Index}";
            } while (parent.Variables.Any(item => item.Name.Equals(varName)));
            return varName;
        }

        private void AddSubSequenceSteps(ISequence parentSequence, ISequenceStepCollection parent, ISequence subSequence)
        {
            ISequence addSequence = (ISequence) subSequence.Clone();
            ChangeCopySequenceNames(addSequence);
            foreach (ISequenceStep step in addSequence.Steps)
            {
                string[] str = step.SubSteps[0].Name.Split(':');
                if (str.Length > 1)
                {
                    ISequence calledSubSeq = _runtimeSequence.Sequences.FirstOrDefault(item => item.Name.Equals(str[1]));
                    if (null == calledSubSeq)
                    {
                        throw new ApplicationException($"Subsequence <{str[1]}> does not exist.");
                    }
                    step.SubSteps.Clear();
                    AddSubSequenceSteps(addSequence, step.SubSteps, calledSubSeq);
                }
            }
            // 拷贝变量到上级，如果存在同名变量且新增的变量配置级别高，则修改为高级别，否则直接添加
            foreach (IVariable subVariable in addSequence.Variables)
            {
                AddVariablesToParent(parentSequence.Variables, subVariable);
            }
            foreach (ISequenceStep subStep in addSequence.Steps)
            {
                parent.Add(subStep);
            }
        }

        private static void AddVariablesToParent(IVariableCollection variableCollection, IVariable subVariable)
        {
            IVariable originalVarible;
            if (null != (originalVarible = variableCollection.FirstOrDefault(item => item.Name.Equals(subVariable.Name))))
            {
                if (subVariable.ReportRecordLevel == RecordLevel.Trace || subVariable.ReportRecordLevel == RecordLevel.FullTrace)
                {
                    originalVarible.ReportRecordLevel = subVariable.ReportRecordLevel;
                }
            }
            else
            {
                variableCollection.Add(subVariable);
            }
        }

        private void AddDefaultInstanceVariable(ISequence runtimeSequence, ISequenceStep constructorStep, ISequenceStep functionStep)
        {
            const string varNamePrefix = "TmpInstanceVar";
            string varName = varNamePrefix;
            int index = 0;
            while (runtimeSequence.Variables.Any(item => item.Name.Equals(varName)))
            {
                varName = varNamePrefix + index++;
            }
            IVariable variable = _globalInfo.TestflowEntity.SequenceManager.CreateVarialbe();
            variable.Name = varName;
            variable.ReportRecordLevel = RecordLevel.None;
            variable.Type = functionStep.Function.ClassType;
            runtimeSequence.Variables.Add(variable);
            constructorStep.Function.Instance = variable.Name;
            functionStep.Function.Instance = variable.Name;
        }

        private void SetUserTimingFlagAndSetDefaultTime(ISequence preUut, ISequence mainSequence, ISequence postUut)
        {
            IVariable timingEnableVar = _runtimeSequence.Variables.FirstOrDefault(item => item.Name.Equals(Constants.TimingEnableVar));
            IVariable startTimeVar = _runtimeSequence.Variables.FirstOrDefault(item => item.Name.Equals(Constants.StartTimeVar));
            IVariable endTimeVar = _runtimeSequence.Variables.FirstOrDefault(item => item.Name.Equals(Constants.EndTimeVar));
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
            if (null == timingEnableVar || !timingEnableVar.Value.Equals(true.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                UserStartTiming = false;
                UserEndTiming = false;
                return;
            }
            UserStartTiming = null != startTimeVar && (HasTimingStepWithSpecifiedVariable(preUut.Steps, Constants.StartTimingMethod, startTimeVar.Name) || HasTimingStepWithSpecifiedVariable(mainSequence.Steps, Constants.StartTimingMethod, startTimeVar.Name));
            UserEndTiming = null != endTimeVar && HasTimingStepWithSpecifiedVariable(mainSequence.Steps, Constants.EndTimingMethod, endTimeVar.Name);
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

        private ISequenceStep GetFirstFunctionStep(ISequenceStepCollection steps, ISequenceFlowContainer parent)
        {
            foreach (ISequenceStep step in steps)
            {
                if (null != step.Function || step.Tag == Constants.ReservedStep)
                {
                    return step;
                }
                if (step.HasSubSteps)
                {
                    ISequenceStep sequenceStep = GetFirstFunctionStep(step.SubSteps, parent);
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
            emptyStep.Parent = parent;
            return emptyStep;
        }

        private ISequenceStep GetLastFunctionStep(ISequenceStepCollection steps, ISequenceFlowContainer parent)
        {
            for (int i = steps.Count - 1; i >= 0; i--)
            {
                ISequenceStep step = steps[i];
                if (null != step.Function || step.Tag == Constants.ReservedStep)
                {
                    return step;
                }
                if (step.HasSubSteps)
                {
                    ISequenceStep sequenceStep = GetLastFunctionStep(step.SubSteps, parent);
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
            emptyStep.Parent = parent;
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
                AddVariablesToParent(runSequence.Variables, variable);
            }
            foreach (IVariable variable in testSequence.Variables)
            {
                AddVariablesToParent(runSequence.Variables, variable);
            }
            foreach (IVariable variable in finallySequence.Variables)
            {
                AddVariablesToParent(runSequence.Variables, variable);
            }

            sequenceManager.ValidateSequenceData(runtimeSequenceGroup);

            // 设置条件循环的loop为Continue变量控制
            IComInterfaceDescription limitDescription = _globalInfo.TestflowEntity.ComInterfaceManager.GetComInterfaceByName("TestStationLimit");
            _globalInfo.TestflowEntity.DesignTimeService.AddComponent(limitDescription);
            IList<IFuncInterfaceDescription> funcList = _globalInfo.TestflowEntity.DesignTimeService.Components["TestStationLimit"].Classes.First(item => item.Name.Equals("Loop")).Functions;
            ISequenceStep loopStep = sequenceManager.CreateNonExecutionStep(SequenceStepType.ConditionLoop);
            loopStep.AssertFailedAction = FailedAction.Terminate;
            loopStep.InvokeErrorAction = FailedAction.Terminate;
            loopStep.Function = _globalInfo.TestflowEntity.SequenceManager.CreateFunctionData(funcList.FirstOrDefault(item => item.Name.Equals("SetBoolValue")));
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
            if ((null != step.LoopCounter && step.LoopCounter.MaxValue > 1) || (null != step.RetryCounter) || step.Tag.Equals(Constants.ReservedStep) || step.StepType == SequenceStepType.ConditionStatement)
            {
                parent.SubSteps.Add(step);
                FailedAction failedAction = step.BreakIfFailed ? defaultFailedAction : FailedAction.Continue;
                step.AssertFailedAction = failedAction;
                step.InvokeErrorAction = failedAction;
                // 如果当前step包含loopCounter或RetryCounter，则需要将下级的SubSteps全部添加到当前step的子节点，否则可以添加到上级的子节点以降低栈调用
                parent = step;
                // 只有预留的Step需要记录状态，其余的都不需要
                step.RecordStatus = step.Tag.Equals(Constants.ReservedStep);
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
                Type boolType = typeof (bool);
                continueVar = _globalInfo.TestflowEntity.SequenceManager.CreateVarialbe();
                continueVar.Name = Constants.ContinueVariable;
                continueVar.Value = "True";
                _runtimeSequence.Variables.Add(continueVar);
                continueVar.Type = _globalInfo.TestflowEntity.ComInterfaceManager.GetTypeByName(boolType.Name, boolType.Namespace);
                continueVar.ReportRecordLevel = RecordLevel.None;
            }

            if (null == _runtimeSequence.Variables.FirstOrDefault(item => item.Name.Equals(Constants.DutIndexVarName)))
            {
                Type intType = typeof (int);
                IVariable dutIndexVar = _globalInfo.TestflowEntity.SequenceManager.CreateVarialbe();
                dutIndexVar.Name = Constants.DutIndexVarName;
                dutIndexVar.Value = "0";
                _runtimeSequence.Variables.Add(dutIndexVar);
                dutIndexVar.Type = _globalInfo.TestflowEntity.ComInterfaceManager.GetTypeByName(intType.Name, intType.Namespace);
                dutIndexVar.ReportRecordLevel = RecordLevel.FullTrace;
            }
        }

        #region 删除拷贝后的序列名称、变量、Step名称等被加上Copy的后缀

        private void ChangeCopySequenceGroupName(ISequenceGroup sequenceData)
        {
            ChangeCopyVariableName(sequenceData.Variables);
            ChangeCopySequenceNames(sequenceData.SetUp);
            ChangeCopySequenceNames(sequenceData.TearDown);
            foreach (ISequence sequence in sequenceData.Sequences)
            {
                ChangeCopySequenceNames(sequence);
            }
        }

        private void ChangeCopySequenceNames(ISequence sequence)
        {
            ChangeCopyVariableName(sequence.Variables);
            sequence.Name = sequence.Name.Replace(CopyItemPostfix, "").Trim();
            foreach (ISequenceStep step in sequence.Steps)
            {
                ChangeCopyStepName(step);
            }
        }

        private void ChangeCopyStepName(ISequenceStep stepData)
        {
            stepData.Name = stepData.Name.Replace(CopyItemPostfix, "").Trim();
            if (null != stepData.SubSteps && stepData.SubSteps.Count > 0)
            {
                foreach (ISequenceStep subStep in stepData.SubSteps)
                {
                    ChangeCopyStepName(subStep);
                }
            }
        }

        private void ChangeCopyVariableName(IVariableCollection variables)
        {
            foreach (IVariable variable in variables)
            {
                if (variable.Name.Contains(CopyItemPostfix))
                {
                    variable.Name = variable.Name.Replace(CopyItemPostfix, "").Trim();
                }
            }
        }

        #endregion

        private void SetStepProperties(ISequenceStep rootStep)
        {
            bool userAssignAction = rootStep.BreakIfFailed;
            rootStep.AssertFailedAction = userAssignAction ? FailedAction.NextLoop : FailedAction.Continue;
            rootStep.InvokeErrorAction = rootStep.AssertFailedAction;
            ;
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