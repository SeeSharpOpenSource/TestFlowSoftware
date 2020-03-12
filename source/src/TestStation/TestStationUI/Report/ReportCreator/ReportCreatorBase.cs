using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Testflow.Data.Sequence;
using Testflow.Runtime.Data;
using TestStation.Common;

namespace TestStation.Report.ReportCreator
{
    public abstract class ReportCreatorBase : IDisposable
    {
        protected const string LimitClassName = "Limit";

        protected Dictionary<string, string> VariableValues { get; }

        protected ReportGlobalInfo ReportInfo { get; }

        protected string ReportDir { get; }

        public static ReportCreatorBase GetReportCreator(ReportType reportType, ReportGlobalInfo reportInfo, string reportDir)
        {
            ReportCreatorBase reportCreator = null;
            switch (reportType)
            {
                case ReportType.Txt:
                    reportCreator = new TxtReportCreator(reportDir, reportInfo);
                    break;
                case ReportType.Html:
                    break;
                case ReportType.Excel:
                    break;
                case ReportType.Word:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reportType), reportType, null);
            }
            return reportCreator;
        }

        protected ReportCreatorBase(string reportDir, ReportGlobalInfo reportInfo)
        {
            this.ReportDir = reportDir;
            this.ReportInfo = reportInfo;
            VariableValues = new Dictionary<string, string>(20);
        }

        protected string GetReportTemplateDir()
        {
            string workspaceDir = Environment.GetEnvironmentVariable("TESTFLOW_WORKSPACE");
            string pathDelim = Path.DirectorySeparatorChar.ToString();
            if (!workspaceDir.EndsWith(pathDelim))
            {
                workspaceDir += pathDelim;
            }
            workspaceDir += $"ReportsTemplate{pathDelim}Desay{pathDelim}";
            return workspaceDir;
        }

        public abstract void SetStartTime(DateTime time);
        public abstract void SetEndTime(DateTime time);

        public virtual void BeginPrint(DateTime time)
        {
            VariableValues.Clear();
        }

        public virtual void PrintSingleInfo(RuntimeStatusData runtimeStatus, ISequenceStep rawStep, ISequenceStep runStep)
        {
            UpdateWatchData(runtimeStatus);
        }

        public virtual void EndPrint()
        {
            VariableValues.Clear();
        }

        private void UpdateWatchData(RuntimeStatusData runtimeStatus)
        {
            if (string.IsNullOrWhiteSpace(runtimeStatus.WatchData))
            {
                return;
            }
            Dictionary<string, string> variables =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(runtimeStatus.WatchData);
            foreach (string variableName in variables.Keys)
            {
                string realVariable = GetShortenVariable(variableName);
                string nextValue = variables[variableName];
                if (VariableValues.ContainsKey(realVariable))
                {
                    // 只有更新的值合法，且原始值不合法时才更新值
                    if (IsValidVariableValue(nextValue) && !IsValidVariableValue(VariableValues[realVariable]))
                    {
                        VariableValues[realVariable] = nextValue;
                    }
                }
                else
                {
                    VariableValues.Add(realVariable, nextValue);
                }
            }
        }

        private string GetShortenVariable(string runtimeVariableName)
        {
            const string runtimeVarDelim = "$";
            int startIndex = runtimeVariableName.LastIndexOf(runtimeVarDelim) + 1;
            if (startIndex <= 0)
            {
                return runtimeVariableName;
            }
            return runtimeVariableName.Substring(startIndex, runtimeVariableName.Length - startIndex);
        }

        protected string GetStepResult(StepResult result)
        {
            switch (result)
            {
                case StepResult.NotAvailable:
                case StepResult.Skip:
                case StepResult.Pass:
                case StepResult.Over:
                    return result.ToString();
                    break;
                case StepResult.RetryFailed:
                case StepResult.Failed:
                case StepResult.Abort:
                case StepResult.Timeout:
                case StepResult.Error:
                    return StepResult.Failed.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }

        private bool IsValidVariableValue(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && !Constants.NASerialNo.Equals(value);
        }

        public abstract void Dispose();

        public abstract void DeleteCurrent();
    }
}