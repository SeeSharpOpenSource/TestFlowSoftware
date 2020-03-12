using System;
using System.Linq;
using System.Text;
using Testflow.Data;
using Testflow.Data.Sequence;

namespace TestStation.Common
{
    internal static class StepInfoCreator
    {
        #region 描述信息生成

        public static void SetStepDescriptionInfo(ISequenceStep step, string stepType)
        {
            switch (stepType)
            {
                case Constants.SeqCallType:
                    step.Description = GetSequenceCallDescription(step, stepType);
                    break;
                case Constants.ActionType:
                    step.Description = GetActionDescription(step, stepType);
                    break;
                case Constants.TestType:
                    step.Description = GetTestDescription(step, stepType);
                    break;
            }
        }

        // Description格式，0-3分别为：step类型、step调用的程序集、step调用的类、step调用的方法
        private const string DescriptionFormat = "[{0}]({1}){2}.{3}";

        private static string GetSequenceCallDescription(ISequenceStep step, string stepType)
        {
            string sequenceCallName = Utility.GetSequenceCallName(step.SubSteps[0].Name);
            return $"[{stepType}] {sequenceCallName}";
        }

        private static string GetActionDescription(ISequenceStep step, string stepType)
        {
            string description = string.Empty;
            ISequenceStep methodStep = step.SubSteps.FirstOrDefault(item => item.Name.Equals("Method"));
            // 如果没有MethodStep，则默认找第二个SubStep作为methodStep
            if (null == methodStep && step.SubSteps.Count > 1)
            {
                methodStep = step.SubSteps[1];
            }
            if (methodStep?.Function != null)
            {
                string assembly = methodStep.Function.ClassType.AssemblyName;
                string className = methodStep.Function.ClassType.Name;
                string methodName = methodStep.Function.MethodName;
                description = string.Format(DescriptionFormat, stepType, assembly, className, methodName);
            }
            return description;
        }

        private static string GetTestDescription(ISequenceStep step, string stepType)
        {
            string description = string.Empty;
            ISequenceStep methodStep = step.SubSteps.FirstOrDefault(item => item.Name.Equals("Method"));
            // 如果没有MethodStep，则默认找第二个SubStep作为methodStep
            if (null == methodStep && step.SubSteps.Count > 1)
            {
                methodStep = step.SubSteps[1];
            }
            if (methodStep?.Function != null)
            {
                string assembly = methodStep.Function.ClassType.AssemblyName;
                string className = methodStep.Function.ClassType.Name;
                string methodName = methodStep.Function.MethodName;
                description = string.Format(DescriptionFormat, stepType, assembly, className, methodName);
            }
            return description;
        }

        #endregion


        #region Setting信息生成

        public static void SetStepSettingInfo(ISequenceStep step, string stepType)
        {
            switch (stepType)
            {
                case Constants.SeqCallType:
                    step.SubSteps[0].Description = GetSequenceCallSetting(step);
                    break;
                case Constants.ActionType:
                    step.SubSteps[0].Description = GetActionSetting(step);
                    break;
                case Constants.TestType:
                    step.SubSteps[0].Description = GetTestSetting(step);
                    break;
            }
        }

        private static string GetSequenceCallSetting(ISequenceStep step)
        {
            return GetCommonSettingInfo(step);
        }

        private static string GetActionSetting(ISequenceStep step)
        {
            return GetCommonSettingInfo(step);
        }

        private static string GetTestSetting(ISequenceStep step)
        {
            return GetCommonSettingInfo(step);
        }

        private static string GetCommonSettingInfo(ISequenceStep step)
        {
            StringBuilder setting = new StringBuilder(100);
            if (null != step.LoopCounter && step.LoopCounter.MaxValue > 1)
            {
                setting.Append("FixedTimesLoop:").Append(step.LoopCounter.MaxValue).Append(";");
            }
            if (null != step.RetryCounter && step.RetryCounter.MaxRetryTimes >= 2)
            {
                setting.Append("PassTimesLoop:")
                    .Append(step.RetryCounter.MaxRetryTimes)
                    .Append("_")
                    .Append(step.RetryCounter.PassTimes)
                    .Append(";");
            }
            if (!step.RecordStatus)
            {
                setting.Append("NoRecord").Append(";");
            }
            if (step.AssertFailedAction == FailedAction.Terminate)
            {
                setting.Append("BreakIfFailed").Append(";");
            }
            if (step.Behavior == RunBehavior.Skip)
            {
                setting.Append("Skip").Append(";");
            }
            return setting.ToString();
        }

        #endregion

    }
}