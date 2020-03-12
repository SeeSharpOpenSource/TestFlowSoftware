using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Testflow.Data;
using Testflow.Data.Sequence;
using TestStation.Common;

namespace TestStation.ParameterChecker
{
    public class SequenceChecker
    {
        private readonly ISequenceGroup _sequenceGroup;
        private readonly ISequence _sequence;
        private readonly Regex _expRegex;
        public SequenceChecker(ISequenceGroup sequenceGroup, ISequence sequence)
        {
            this._sequenceGroup = sequenceGroup;
            this._sequence = sequence;
            this._expRegex = new Regex("^(.+)\\[\\d+\\]$");
        }

        public void Check(IList<string> errorInfoCache)
        {
            foreach (ISequenceStep sequenceStep in _sequence.Steps)
            {
                CheckRootStep(sequenceStep, errorInfoCache);
            }
        }

        private void CheckRootStep(ISequenceStep rootStep, IList<string> errorInfoCache)
        {
            string stepType = GetStepType(rootStep);
            switch (stepType)
            {
                case "":
                    break;
                case Constants.TestType:
                    CheckTestStep(rootStep, errorInfoCache);
                    break;
                case Constants.ActionType:
                    CheckActionStep(rootStep, errorInfoCache);
                    break;
                case Constants.SeqCallType:
                    CheckSeqCallStep(rootStep, errorInfoCache);
                    break;
            }
        }

        private void CheckTestStep(ISequenceStep rootStep, IList<string> errorInfoCache)
        {
            for (int i = 1; i < rootStep.SubSteps.Count; i++)
            {
                ISequenceStep subStep = rootStep.SubSteps[i];
                if (null != subStep.Function && subStep.Function.Type == FunctionType.Constructor)
                {
                    CheckConstructor(rootStep, subStep, errorInfoCache);
                }
                else if (subStep.Name.Equals(Constants.MethodStepName) || subStep.Name.StartsWith(Constants.LimitStepPrefix))
                {
                    CheckMethod(rootStep, subStep, errorInfoCache);
                }
            }
        }

        private void CheckActionStep(ISequenceStep rootStep, IList<string> errorInfoCache)
        {
            for (int i = 1; i < rootStep.SubSteps.Count; i++)
            {
                ISequenceStep subStep = rootStep.SubSteps[i];
                if (null != subStep.Function && subStep.Function.Type == FunctionType.Constructor)
                {
                    CheckConstructor(rootStep, subStep, errorInfoCache);
                }
                else if (subStep.Name.Equals(Constants.MethodStepName) || subStep.Name.StartsWith(Constants.LimitStepPrefix))
                {
                    CheckMethod(rootStep, subStep, errorInfoCache);
                }
            }
        }

        private void CheckSeqCallStep(ISequenceStep rootStep, IList<string> errorInfoCache)
        {
            if (rootStep.SubSteps.Count != 1)
            {
                errorInfoCache.Add($"Sequence <{_sequence.Name}> Step <{rootStep.Name}>: Contains invalid substeps.");
                return;
            }
            ISequenceStep typeStep = rootStep.SubSteps[0];
            string[] nameElement = typeStep.Name.Split(':');
            if (nameElement.Length != 2)
            {
                errorInfoCache.Add($"Sequence <{_sequence.Name}> Step <{rootStep.Name}>: Reference to invalid sequence.");
                return;
            }
            string refSeqName = nameElement[1];
            ISequence refSeq = _sequenceGroup.Sequences.FirstOrDefault(item => item.Name.Equals(refSeqName));
            if (null == refSeq)
            {
                errorInfoCache.Add($"Sequence <{_sequence.Name}> Step <{rootStep.Name}>: Calls an unexist sequence <{refSeqName}>.");
                return;
            }
            if (refSeq.Index <= 2)
            {
                errorInfoCache.Add($"Sequence <{_sequence.Name}> Step <{rootStep.Name}>: Calls an invalid sequence <{refSeqName}>.");
            }
        }

        private void CheckConstructor(ISequenceStep rootStep, ISequenceStep constructStep, IList<string> errorInfoCache)
        {
            IFunctionData functionData = constructStep.Function;
            if (null == functionData)
            {
                errorInfoCache.Add($"Sequence <{_sequence.Name}> Step <{rootStep.Name}>: The constructor function has not configured.");
                return;
            }
            for (int i = 0; i < functionData.Parameters.Count; i++)
            {
                CheckArgument(rootStep, functionData, i, errorInfoCache);
            }
        }

        private void CheckMethod(ISequenceStep rootStep, ISequenceStep methodStep, IList<string> errorInfoCache)
        {
            IFunctionData functionData = methodStep.Function;
            if (null == functionData)
            {
                errorInfoCache.Add($"Sequence <{_sequence.Name}> Step <{rootStep.Name}>: Method function is not configured.");
                return;
            }
            // 如果实例变量未包含在变量定义中，则抛出错误
            string returnValue = functionData.Return;
            bool isVariableExist = IsVariableExist(returnValue);
            if (null != functionData.ReturnType && !string.IsNullOrWhiteSpace(returnValue) && !isVariableExist)
            {
                errorInfoCache.Add($"Sequence <{_sequence.Name}> step <{rootStep.Name}>: The return variable is not exist.");
            }
            bool isInstanceFunc = functionData.Type == FunctionType.InstanceFieldSetter ||
                                  functionData.Type == FunctionType.InstanceFunction ||
                                  functionData.Type == FunctionType.InstancePropertySetter;
            // 如果方法是实例方法且未配置类实例，且上级步骤不存在该类的构造方法，则返回错误提示信息
            string instanceValue = functionData.Instance;
            if (isInstanceFunc && string.IsNullOrWhiteSpace(instanceValue) &&
                !rootStep.SubSteps.Any(item =>
                        null != item.Function && item.Function.Type == FunctionType.Constructor &&
                        item.Function.ClassType.Equals(functionData.ClassType)))
            {
                errorInfoCache.Add($"Sequence <{_sequence.Name}> Step <{rootStep.Name}>: The instance of function is not configured.");
                return;
            }
            // 如果实例变量未包含在变量定义中，则抛出错误
            if (isInstanceFunc && !_sequence.Variables.Any(item => item.Name.Equals(instanceValue)) &&
                !_sequenceGroup.Variables.Any(item => item.Name.Equals(instanceValue)))
            {
                errorInfoCache.Add($"Sequence <{_sequence.Name}> Step <{rootStep.Name}>: The instance variable does not exist.");
            }
            if (null != functionData.Parameters)
            {
                for (int i = 0; i < functionData.Parameters.Count; i++)
                {
                    CheckArgument(rootStep, functionData, i, errorInfoCache);
                }
            }
            
        }

        private bool IsVariableExist(string variableName)
        {
            return _sequence.Variables.Any(item => item.Name.Equals(variableName)) ||
                   _sequenceGroup.Variables.Any(item => item.Name.Equals(variableName));
        }

        private void CheckArgument(ISequenceStep rootStep, IFunctionData function, int argIndex, IList<string> errorInfoCache)
        {
            
            IArgument argument = function.ParameterType[argIndex];
            IParameterData argParam = function.Parameters[argIndex];
            // 非Setter方法的某个参数未配置
            switch (argParam.ParameterType)
            {
                case ParameterType.NotAvailable:
                    bool isSetterFunction = function.Type == FunctionType.InstanceFieldSetter || function.Type == FunctionType.StaticFieldSetter ||
                                            function.Type == FunctionType.InstancePropertySetter ||
                                            function.Type == FunctionType.StaticPropertySetter;
                    if (!isSetterFunction)
                    {
                        errorInfoCache.Add($"Sequence <{_sequence.Name}> Step <{rootStep.Name}>: The value of argument <{argument.Name}> is not configured.");
                    }
                    return;
                    break;
                case ParameterType.Expression:
                    Match matchData = _expRegex.Match(argParam.Value);
                    if (!matchData.Success)
                    {
                        errorInfoCache.Add(
                            $"Sequence <{_sequence.Name}> Step <{rootStep.Name}>: The value of argument <{argument.Name}> is invalid.");
                        return;
                    }
                    string variable = matchData.Groups[1].Value;
                    if (!IsVariableExist(variable))
                    {
                        errorInfoCache.Add(
                            $"Sequence <{_sequence.Name}> Step <{rootStep.Name}>: The variable <{variable}> of Argument <{argument.Name}> value is not exist.");
                    }
                    break;
                case ParameterType.Value:
                    if (!CheckValueType(argument.Type, argParam.Value))
                    {
                        errorInfoCache.Add($"Sequence <{_sequence.Name}> Step <{rootStep.Name}>: The value of argument <{argument.Name}> is invalid.");
                    }
                    break;
                case ParameterType.Variable:
                    if (!IsVariableExist(argParam.Value))
                    {
                        errorInfoCache.Add(
                            $"Sequence <{_sequence.Name}> Step <{rootStep.Name}>: The variable <{argParam.Value}> of Argument <{argument.Name}> value is not exist.");
                    }
                    break;
            }
        }

        private bool CheckValueType(ITypeData type, string value)
        {
            if (type.Name.Equals(typeof(double).Name))
            {
                double parseValue;
                return double.TryParse(value, out parseValue);
            }
            if (type.Name.Equals(typeof(float).Name))
            {
                float parseValue;
                return float.TryParse(value, out parseValue);
            }
            if (type.Name.Equals(typeof(int).Name))
            {
                int parseValue;
                return int.TryParse(value, out parseValue);
            }
            if (type.Name.Equals(typeof(uint).Name))
            {
                uint parseValue;
                return uint.TryParse(value, out parseValue);
            }
            if (type.Name.Equals(typeof(short).Name))
            {
                short parseValue;
                return short.TryParse(value, out parseValue);
            }
            if (type.Name.Equals(typeof(ushort).Name))
            {
                ushort parseValue;
                return ushort.TryParse(value, out parseValue);
            }
            if (type.Name.Equals(typeof(byte).Name))
            {
                byte parseValue;
                return byte.TryParse(value, out parseValue);
            }
            if (type.Name.Equals(typeof(char).Name))
            {
                char parseValue;
                return char.TryParse(value, out parseValue);
            }
            if (type.Name.Equals(typeof(bool).Name))
            {
                bool parseValue;
                return bool.TryParse(value, out parseValue);
            }
            return true;
        }

        private static string GetStepType(ISequenceStep rootStep)
        {
            if (null == rootStep.SubSteps || rootStep.SubSteps.Count < 1)
            {
                return string.Empty;
            }
            ISequenceStep typeStep = rootStep.SubSteps[0];
            if (typeStep.Name.Equals(Constants.TestType) || typeStep.Name.Equals(Constants.ActionType))
            {
                return typeStep.Name;
            }
            else if (typeStep.Name.StartsWith(Constants.SeqCallType))
            {
                return Constants.SeqCallType;
            }
            return string.Empty;
        }
    }
}