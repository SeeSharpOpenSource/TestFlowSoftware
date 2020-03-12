using System;
using System.IO;
using System.Text;
using Testflow.Data.Sequence;
using Testflow.Runtime.Data;
using TestStation.Common;

namespace TestStation.Report.ReportCreator
{
    public class TxtReportCreator : ReportCreatorBase
    {
        private readonly string _txtTemplateDir;

        private StreamWriter _writeStream;

        private readonly StringBuilder _itemBuffer;
        private readonly StringBuilder _stepBuffer;

        private const string StepTarget = "StepTemplate";
        private const string HeaderTarget = "HeaderTemplate";
        private const string LimitTarget = "LimitTemplate";
        private const string EndTarget = "EndTemplate";

        private string _stepTemplate = "StepTemplate";
        private string _headerTemplate = "HeaderTemplate";
        private string _limitTemplate = "LimitTemplate";
        private string _endTemplate = "EndTemplate";

        private long _stepRunTicks = 0;

        private bool _uutPassed;
        private bool _stepPassed;
        private string _lastSerialNumber;
        private string _serialNumber;
        private DateTime _startTime;
        private DateTime _endTime;

        private const string FileNameFormat = "{0}uut{1}.txt";
        private int _uutIndex;
        private string _currentReportPath;
        private const string NullValue = "NULL";
        private const string ReportNameFormat = "ReportGeneratorTest_Report_[{0}][{1}]_{2}{3}.txt";

        public TxtReportCreator(string reportDir, ReportGlobalInfo reportInfo) : base(reportDir, reportInfo)
        {
            string templateDir = GetReportTemplateDir();
            _txtTemplateDir = $"{templateDir}{Path.DirectorySeparatorChar}txtReport{Path.DirectorySeparatorChar}";
            _itemBuffer = new StringBuilder(1000);
            _stepBuffer = new StringBuilder(2000);

            _stepTemplate = GetTargetTemplate(StepTarget);
            _headerTemplate = GetTargetTemplate(HeaderTarget);
            _limitTemplate = GetTargetTemplate(LimitTarget);
            _endTemplate = GetTargetTemplate(EndTarget);
            _uutIndex = 0;
            _uutPassed = true;
            ClearParams();
            _lastSerialNumber = NullValue;
            _serialNumber = NullValue;
        }


        public override void SetStartTime(DateTime time)
        {
            _startTime = time;
        }

        public override void SetEndTime(DateTime time)
        {
            _endTime = time;
        }

        public override void BeginPrint(DateTime time)
        {
            base.BeginPrint(time);
            _writeStream?.Dispose();
            _currentReportPath = string.Format(FileNameFormat, ReportDir, _uutIndex);
            _writeStream = new StreamWriter(_currentReportPath);
            _startTime = time;
            RefreshSerialNumber();
        }

        public override void PrintSingleInfo(RuntimeStatusData runtimeStatus, ISequenceStep rawStep, ISequenceStep runStep)
        {
            base.PrintSingleInfo(runtimeStatus, rawStep, runStep);
            // 如果rawStep为null，则说明该Step为SequenceMaintainer自己添加的序列Step，无关运行，不维护该状态
            if (null == rawStep)
            {
                return;
            }
            _endTime = runtimeStatus.ExecutionTime + new TimeSpan(runtimeStatus.ExecutionTicks);
            StepResult stepResult = runtimeStatus.Result;
            // 如果是LimitStep
            if (null != runStep.Function && runStep.Function.ClassType.Name.Equals(LimitClassName))
            {
                IParameterDataCollection parameters = runStep.Function.Parameters;
                InitItemBuffer(_limitTemplate);
                FillTemplateString("LimitName", runStep.Description);
                string variable = parameters[3].Value;
                FillTemplateString("VariableName", variable);
                
                FillTemplateString("Result", GetStepResult(stepResult));
                FillTemplateString("LowValue", parameters[0].Value);
                FillTemplateString("HighValue", parameters[1].Value);
                FillTemplateString("Unit", parameters[4].Value);
                FillTemplateString("ComparisonType", parameters[2].Value);
                FillTemplateString("Value", VariableValues.ContainsKey(variable) ? VariableValues[variable] : "NULL");
                _stepPassed = IsStepPassed(stepResult);
                _uutPassed &= _stepPassed;
                MoveToStepBuffer();
            }
            else
            {
                if (_stepBuffer.Length > 0)
                {
                    FillStepBufferString("Result", stepResult.ToString());
                    FillStepBufferString("ExecutionSeconds", (_stepRunTicks/1E7).ToString("F7"));
                    _stepRunTicks = 0;
                    _writeStream.WriteLine(_stepBuffer);
                    _stepBuffer.Clear();
                }
                _stepPassed = IsStepPassed(runtimeStatus.Result);
                _uutPassed &= _stepPassed;
                InitItemBuffer(_stepTemplate);
                FillTemplateString("SequenceName", rawStep.Name);
                _stepRunTicks = runtimeStatus.ExecutionTicks;
                MoveToStepBuffer();
            }
            RefreshSerialNumber();
        }

        private static bool IsStepPassed(StepResult stepResult)
        {
            return stepResult == StepResult.Pass || stepResult == StepResult.RetryFailed ||
                   stepResult == StepResult.Skip;
        }

        public override void EndPrint()
        {
            if (_stepBuffer.Length > 0)
            {
                FillStepBufferString("Result", _uutPassed ? StepResult.Pass.ToString() : StepResult.Failed.ToString());
                FillStepBufferString("ExecutionSeconds", (_stepRunTicks / 1E7).ToString("F7"));
                _stepRunTicks = 0;
                _writeStream.WriteLine(_stepBuffer);
                _stepBuffer.Clear();
            }
            DateTime time;
            if (VariableValues.ContainsKey(Constants.StartTimeVar) &&
                DateTime.TryParse(VariableValues[Constants.StartTimeVar], out time))
            {
                _startTime = time;
            }
            if (VariableValues.ContainsKey(Constants.EndTimeVar) &&
                DateTime.TryParse(VariableValues[Constants.EndTimeVar], out time))
            {
                _endTime = time;
            }
            _writeStream.WriteLine(_endTemplate);
            _writeStream.Flush();
            _writeStream.Close();
            // 如果串号非法，则说明当前测试无效，删除已写入的内容
            if (_serialNumber.Equals(Constants.NASerialNo) || _serialNumber.Equals(NullValue))
            {
                _writeStream?.Dispose();
                if (!string.IsNullOrWhiteSpace(_currentReportPath) && File.Exists(_currentReportPath))
                {
                    File.Delete(_currentReportPath);
                }
            }
            else
            {
                string uutResult = _uutPassed ? "Pass" : "Failed";
                string reportBody = File.ReadAllText(_currentReportPath);
                InitItemBuffer(_headerTemplate);
                FillTemplateString("Station", ReportInfo.StationId);
                FillTemplateString("SocketIndex", ReportInfo.SocketIndex);
                FillTemplateString("SerialNumber", _serialNumber);
                FillTemplateString("Date", _startTime.ToString("yyyy-MM-dd"));
                FillTemplateString("Time", _startTime.ToString("HH:mm:ss.fff"));
                FillTemplateString("Operator", ReportInfo.Operator);
                FillTemplateString("ExecutionTime", (_endTime - _startTime).TotalSeconds.ToString("F3"));
                FillTemplateString("UUTResult", uutResult);
                FillTemplateString("SequencePath", ReportInfo.SequencePath);
                
                _writeStream = new StreamWriter(_currentReportPath);
                _writeStream.BaseStream.SetLength(0);
                _writeStream.Flush();
                _writeStream.WriteLine(_itemBuffer);
                _writeStream.WriteLine(reportBody);
                _writeStream.Close();
                _writeStream.Dispose();
                _writeStream = null;
                ClearItemBuffer();
                string reportName = string.Format(ReportNameFormat, _uutIndex + 1,
                    _startTime.ToString("yyyy-MM-dd HH-mm-ss"), uutResult, "");
                int offset = 0;
                while (File.Exists(reportName))
                {
                    reportName = string.Format(ReportNameFormat, _uutIndex + 1,
                    _startTime.ToString("yyyy-MM-dd HH-mm-ss"), uutResult, "_"+offset++.ToString());
                }
                string fullPath = ReportDir + reportName;
                File.Move(_currentReportPath, fullPath);
                _currentReportPath = fullPath;
            }
            _lastSerialNumber = _serialNumber;
            _currentReportPath = string.Empty;
            ClearParams();
            _uutIndex++;
            base.EndPrint();
        }

        private void ClearParams()
        {
            _uutPassed = true;
            _startTime = DateTime.MinValue;
            _endTime = DateTime.MinValue;
            _serialNumber = Constants.NASerialNo;
            _stepBuffer.Clear();
            _itemBuffer.Clear();
        }

        private string GetTargetTemplate(string target)
        {
            string templateDir = $"{_txtTemplateDir}{target}.txt";
            string template;
            using (StreamReader templateReader = new StreamReader(templateDir))
            {
                template = templateReader.ReadToEnd();
            }
            return template;
        }

        private void FillTemplateString(string label, string value)
        {
            string labelFormat = "{{{0}}}";
            _itemBuffer.Replace(string.Format(labelFormat, label), value);
        }

        private void FillStepBufferString(string label, string value)
        {
            string labelFormat = "{{{0}}}";
            _stepBuffer.Replace(string.Format(labelFormat, label), value);
        }

        private void MoveToStepBuffer()
        {
            _stepBuffer.Append(Environment.NewLine).Append(_itemBuffer);
            _itemBuffer.Clear();
        }

        private void InitItemBuffer(string template)
        {
            _itemBuffer.Clear();
            _itemBuffer.Append(template);
        }

        private void RefreshSerialNumber()
        {
            if (VariableValues.ContainsKey(Constants.SerialNoVarName))
            {
                string serialNumber = VariableValues[Constants.SerialNoVarName];
                if (!serialNumber.Equals(_lastSerialNumber) && !serialNumber.Equals(_serialNumber) && !string.IsNullOrWhiteSpace(serialNumber) && 
                    !serialNumber.Equals(NullValue) && !serialNumber.Equals(Constants.NASerialNo))
                {
                    _serialNumber = serialNumber;
                }
            }
        }

        private void ClearItemBuffer()
        {
            _itemBuffer.Clear();
        }

        public override void Dispose()
        {
            _writeStream?.Dispose();
        }

        public override void DeleteCurrent()
        {
            ClearParams();
            try
            {
                _writeStream?.Close();
                if (File.Exists(_currentReportPath))
                {
                    File.Delete(_currentReportPath);
                }
            }
            catch (IOException ex)
            {
                Log.Print(LogLevel.WARN, $"Delete report {_currentReportPath} failed.{ex.Message}");
            }
        }
    }
}