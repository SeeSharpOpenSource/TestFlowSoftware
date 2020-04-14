using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Testflow.Data;
using Testflow.Data.Description;
using Testflow.Data.Sequence;
using Testflow.Modules;

namespace TestStation.Common
{
    public static class Utility
    {
        public static string GetShowVariableName(IVariable variable)
        {
            string variableName = variable.Name;
            bool isGlobalVariable = !(variable.Parent is ISequence);
            return GetShowVariableName(isGlobalVariable, variableName);
        }


        public static bool IsActionStep(ISequenceStep step)
        {
            return step.HasSubSteps && step.SubSteps[0].Name.Equals(Constants.ActionType);
        }

        public static string GetShowVariableName(string variableName, ISequenceStep functionStep)
        {
            if (null == functionStep || string.IsNullOrWhiteSpace(variableName))
            {
                return string.Empty;
            }
            ISequenceStep step = functionStep;
            while (step.Parent is ISequenceStep)
            {
                step = (ISequenceStep) functionStep.Parent;
            }
            bool isGlobalVariable = !((step.Parent as ISequence)?.Variables.Any(item => item.Name.Equals(variableName)) ?? false);
            return GetShowVariableName(isGlobalVariable, variableName);
        }

        public static string GetShowVariableName(bool isGlobalVariable, string variableName)
        {
            if (isGlobalVariable)
            {
                return $"{Constants.GlobalVarPrefix}{Constants.VaraibleDelim}{variableName}";
            }
            else
            {
                return $"{Constants.LocalVarPrefix}{Constants.VaraibleDelim}{variableName}";
            }
        }

        public static string GetVariableName(string paramValue)
        {
            string variableName = paramValue;
            char delim = '.';
            if (paramValue.Contains(delim))
            {
                variableName = paramValue.Substring(0, paramValue.IndexOf(delim));
            }
            return variableName;
        }

        public static string GetParamValue(string tableValue, out bool isVariable)
        {
            if (tableValue.StartsWith(Constants.GlobalVarPrefix + ".", StringComparison.OrdinalIgnoreCase) ||
                tableValue.StartsWith(Constants.LocalVarPrefix + ".", StringComparison.OrdinalIgnoreCase))
            {
                isVariable = true;
                return tableValue.Substring(tableValue.IndexOf(Constants.VaraibleDelim) + 1);
            }
            else
            {
                isVariable = false;
                return tableValue;
            }
        }

        // 简单判断字符串是否为json
        public static bool IsJsonValue(string value)
        {
            return (value.StartsWith("{") && value.EndsWith("}")) || (value.StartsWith("[") && value.EndsWith("]"));
        }

        public static bool IsSequenceCall(ISequenceStep step)
        {
            if (null != step.SubSteps && step.SubSteps.Count > 0)
            {
                return step.SubSteps[0].Name.StartsWith($"{Constants.SeqCallType}:");
            }
            return false;
        }

        public static string GetSequenceCallName(string stepName)
        {
            string[] nameElem = stepName.Split(':');
            return nameElem.Length == 2 ? nameElem[1] : string.Empty;
        }

        public static ISequence GetParentSequence(ISequenceStep step)
        {
            while (step.Parent is ISequenceStep)
            {
                step = (ISequenceStep)step.Parent;
            }
            return (ISequence)step.Parent;
        }

        public static string GetStepType(ISequenceStep step)
        {
            if (null == step.SubSteps || step.SubSteps.Count == 0)
            {
                return Constants.ActionType;
            }
            ISequenceStep typeStep = step.SubSteps[0];
            return typeStep.Name.Contains(Constants.SeqCallType) ? Constants.SeqCallType : typeStep.Name;
        }

        public static ISequenceStep GetLimitStep(ISequenceStep step)
        {
            if (null == step.SubSteps || step.SubSteps.Count < 3)
            {
                return null;
            }
            for (int i = 2; i < step.SubSteps.Count; i++)
            {
                if (step.SubSteps[i].Name.Contains("Limit"))
                {
                    return step.SubSteps[i];
                }
            }
            return null;
        }

        public static IVariable GetVariable(string variableName, ISequenceStep step)
        {
            IVariable variable = null;
            ISequenceStep currentStep = step;
            while (currentStep.Parent is ISequenceStep)
            {
                currentStep = (ISequenceStep)currentStep.Parent;
            }
            ISequence sequence = (ISequence)currentStep.Parent;
            if (null != (variable = sequence.Variables.FirstOrDefault(item => item.Name.Equals(variableName))))
            {
                return variable;
            }
            ISequenceGroup sequenceGroup = (ISequenceGroup)sequence.Parent;
            return sequenceGroup.Variables.FirstOrDefault(item => item.Name.Equals(variableName));
        }

        public static string GetStepStack(int session, ISequenceStep step)
        {
            StringBuilder stacks = new StringBuilder(10);
            stacks.Append("_").Append(step.Index);
            while (step.Parent is ISequenceStep)
            {
                step = (ISequenceStep) step.Parent;
                stacks.Insert(0, step.Index).Insert(0, "_");
            }

            ISequence sequence = (ISequence) step.Parent;
            stacks.Insert(0, sequence.Index).Insert(0, "_");
            stacks.Insert(0, session);
            return stacks.ToString();
        }

        public static string GetReportDir(string reportDir, string sequenceName, DateTime time)
        {
            string pathDelim = Path.DirectorySeparatorChar.ToString();
            StringBuilder reportPath = new StringBuilder(reportDir);
            if (!reportDir.EndsWith(pathDelim))
            {
                reportPath.Append(pathDelim);
            }
            reportPath.Append(sequenceName).Append(pathDelim);
            reportDir = reportPath.ToString();
            // 创建SequenceName命名的文件夹
            if (!Directory.Exists(reportDir))
            {
                Directory.CreateDirectory(reportDir);
            }
            reportPath.Append($"{time.ToString("yyyyMMdd")}{pathDelim}");
            reportDir = reportPath.ToString();
            // 创建运行日期命名的文件夹
            if (!Directory.Exists(reportDir))
            {
                Directory.CreateDirectory(reportDir);
            }
            return reportDir;
        }

        public static bool IsValueType(IArgument argument)
        {
            string typeFullName = GetTypeFullName(argument.Type.Namespace, argument.Type.Name);
            return typeFullName.Equals(GetTypeFullName(typeof (string))) ||
                   typeFullName.Equals(GetTypeFullName(typeof (long))) ||
                   typeFullName.Equals(GetTypeFullName(typeof (ulong))) ||
                   typeFullName.Equals(GetTypeFullName(typeof (int))) ||
                   typeFullName.Equals(GetTypeFullName(typeof (uint))) ||
                   typeFullName.Equals(GetTypeFullName(typeof (short))) ||
                   typeFullName.Equals(GetTypeFullName(typeof (ushort))) ||
                   typeFullName.Equals(GetTypeFullName(typeof (byte))) ||
                   typeFullName.Equals(GetTypeFullName(typeof (char))) ||
                   typeFullName.Equals(GetTypeFullName(typeof (decimal))) ||
                   typeFullName.Equals(GetTypeFullName(typeof (DateTime))) ||
                   typeFullName.Equals(GetTypeFullName(typeof (double))) ||
                   typeFullName.Equals(GetTypeFullName(typeof (float))) ||
                   typeFullName.Equals(GetTypeFullName(typeof (bool)));
        }

        public static IClassInterfaceDescription GetElementInterface(IComInterfaceManager interfaceManager,
            ITypeData typeData)
        {
            if (!IsArrayType(typeData))
            {
                return null;
            }
            string elementName = typeData.Name.Replace("[]", "");
            IList<IComInterfaceDescription> descriptions = interfaceManager.GetComponentDescriptions();
            foreach (IComInterfaceDescription description in descriptions)
            {
                foreach (IClassInterfaceDescription classDescription in description.Classes)
                {
                    if (classDescription.ClassType.Name.Equals(elementName, StringComparison.OrdinalIgnoreCase))
                    {
                        return classDescription;
                    }
                }
            }
            return null;
        }

        public static bool IsArrayType(ITypeData typeData)
        {
            return typeData.Name.EndsWith("[]");
        }

        private static string GetTypeFullName(string namespaceStr, string typeName)
        {
            return $"{namespaceStr}.{typeName}";
        }

        private static string GetTypeFullName(Type dataType)
        {
            return $"{dataType.Namespace}.{dataType.Name}";
        }

        public static string GetSequenceName(string fileName)
        {
            char pathDelim = Path.DirectorySeparatorChar;
            const char extensionDelim = '.';
            if (fileName.Contains(pathDelim))
            {
                int nameStartIndex = fileName.LastIndexOf(pathDelim);
                fileName = fileName.Substring(nameStartIndex + 1, fileName.Length - nameStartIndex - 1);
            }
            if (fileName.Contains(extensionDelim))
            {
                int nameEndIndex = fileName.LastIndexOf(extensionDelim);
                fileName = fileName.Substring(0, nameEndIndex);
            }
            return fileName;
        }

        public static string GetConditionStepDescription(string assertType, string variable)
        {
            if (variable.StartsWith($"{Constants.GlobalVarPrefix}.", StringComparison.OrdinalIgnoreCase) ||
                variable.StartsWith($"{Constants.LocalVarPrefix}."))
            {
                int delimIndex = variable.IndexOf('.');
                variable = variable.Substring(delimIndex + 1, variable.Length - delimIndex - 1);
            }
            return $"{assertType}%{variable}";
        }

        public static void GetConditionProperty(string conditionStepDesp, out string assertType, out string variable)
        {
            string[] despElems = conditionStepDesp.Split('%');
            if (despElems.Length != 2)
            {
                assertType = string.Empty;
                variable = string.Empty;
                return;
            }
            assertType = despElems[0];
            variable = despElems[1];
        }

        public static bool IsPreUutStack(string stack, string mainStartStack)
        {
            string[] stackElem = stack.Split('_');
            string[] compareElem = mainStartStack.Split('_');
            bool isEquals = true;
            if (!stackElem[0].Equals("0"))
            {
                return false;
            }
            int compareSize = stackElem.Length > compareElem.Length ? compareElem.Length : stackElem.Length;
            for (int i = 0; i < compareSize; i++)
            {
                int stackElemValue = int.Parse(stackElem[i]);
                int compElemValue = int.Parse(compareElem[i]);
                if (stackElemValue > compElemValue)
                {
                    return false;
                }
                isEquals &= stackElemValue == compElemValue;
            }
            // 如果Stack的长度比比较的短，也说明比Stack小
            return !isEquals || stackElem.Length < compareElem.Length;
        }

        public static bool IsPostUutStack(string stack, string mainOverStack)
        {
            if (stack.Equals(mainOverStack))
            {
                return true;
            }
            string[] stackElem = stack.Split('_');
            string[] compareElem = mainOverStack.Split('_');
            if (!stackElem[0].Equals("0"))
            {
                return false;
            }
            bool isEqual = true;
            int compareSize = stackElem.Length > compareElem.Length ? compareElem.Length : stackElem.Length;
            for (int i = 0; i < compareSize; i++)
            {
                int stackElemValue = int.Parse(stackElem[i]);
                int compElemValue = int.Parse(compareElem[i]);
                if (stackElemValue < compElemValue)
                {
                    return false;
                }
                isEqual &= stackElemValue == compElemValue;
            }
            // 如果Stack的长度比比较的长，也说明比Stack大
            return stackElem.Length >= compareElem.Length;
        }
    }
}