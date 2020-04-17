using System;
using System.IO;
using System.Linq;
using System.Text;
using Testflow.Data;
using Testflow.Data.Sequence;
using Testflow.Runtime;
using Testflow.Runtime.Data;
using Testflow.Usr;
using TestFlow.SoftDevCommon;
using TestFlow.SoftDSevCommon;

namespace TestFlow.DevSoftware.Report.ReportCreator
{
    public class TxtReportCreator : ReportCreatorBase
    {
        private readonly string _txtTemplateDir;
        private static char[] InvalidPathChars = {'\\', '/', ':', '*', '?', '"', '<', '>', '|'};
        private const char InvalidCharReplacer = '_';

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
        private bool _uutError;
        private bool _stepPassed;
        private bool _stepError;
        private bool _isActionStep;
        private string _lastSerialNumber;
        private string _serialNumber;
        private DateTime _startTime;
        private DateTime _endTime;
        private string _lastStepName;
        private IFailedInfo _failedInfo;

        private int _uutIndex;
        // 对内使用，不使用volatile保护
        private string _currentReportPath;
        private const string NullValue = "NULL";

        public TxtReportCreator(string reportDir, ReportGlobalInfo reportInfo) : 
            base(reportDir, reportInfo)
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
            _uutError = false;
            _stepPassed = true;
            _stepError = false;
            ClearParams();
            _lastSerialNumber = NullValue;
            _serialNumber = NullValue;
            _currentReport = string.Empty;
            _isActionStep = true;
        }

        // 对外使用，使用volatile保护
        private volatile string _currentReport;
        public override string CurrentReport => _currentReport;

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
            _currentReportPath = GetAvailableTemporaryPath();
            _writeStream = new StreamWriter(_currentReportPath);
            _startTime = time;
            _stepPassed = true;
            _stepError = false;
            RefreshSerialNumber();
            _lastStepName = string.Empty;
            _uutPassed = true;
            _uutError = false;
            _failedInfo = null;
        }

        public override void PrintSingleInfo(RuntimeStatusData runtimeStatus, ISequenceStep rawStep, ISequenceStep runStep)
        {
            base.PrintSingleInfo(runtimeStatus, rawStep, runStep);
            bool retryEnabled = null != rawStep?.RetryCounter && rawStep.RetryCounter.MaxRetryTimes > 1;
            RefreshSerialNumber();
            // 如果StepBuffer中包含内容，且当前step名称和上次的不同，则写入上次Step的执行结果内容后再写入当前的
            if (_stepBuffer.Length > 0 && (null == rawStep || !_lastStepName.Equals(rawStep.Name)))
            {
                FillStepBufferString("Result", GetCurrentStepResult());
                FillStepBufferString("ExecutionSeconds", (_stepRunTicks / 1E7).ToString("F7"));
                _stepRunTicks = 0;
                _writeStream.WriteLine(_stepBuffer);
                _stepBuffer.Clear();
                _stepPassed = true;
                _stepError = false;
                _stepRunTicks = 0;
                _isActionStep = true;
            }
            // 如果使能retry并且命名和上一个step相同，则执行时间想加，否则等于status的时间
            if (retryEnabled && _lastStepName.Equals(rawStep.Name))
            {
                _stepRunTicks += runtimeStatus.ExecutionTicks;
            }
            else
            {
                _stepRunTicks = runtimeStatus.ExecutionTicks;
            }
            // 如果rawStep为null，或者该条记录不应该记录状态则返回
            if (null == rawStep || !ShouldStatusBeRecord(runtimeStatus, rawStep))
            {
                _lastStepName = string.Empty;
                return;
            }
            // 如果记录是RetryFailed，该记录是重试超过次数后返回的，不执行记录
            if (runtimeStatus.Result == StepResult.RetryFailed)
            {
                _lastStepName = rawStep.Name;
                return;
            }
            _endTime = runtimeStatus.ExecutionTime + new TimeSpan(runtimeStatus.ExecutionTicks);
            StepResult stepResult = runtimeStatus.Result;
            // 如果是LimitStep
            bool isLimitStep = null != runStep.Function && runStep.Function.ClassType.Name.Equals(LimitClassName) &&
                     runStep.Function.MethodName.StartsWith("Assert");
            if (isLimitStep)
            {
                PrintLimit(runtimeStatus, rawStep, runStep, stepResult);
            }
            else
            {
                PrintStepInfo(runtimeStatus, rawStep, runStep, retryEnabled);
                _isActionStep = Utility.IsActionStep(rawStep);
            }
            _lastStepName = rawStep.Name;
        }

        private void PrintLimit(RuntimeStatusData runtimeStatus, ISequenceStep rawStep, ISequenceStep runStep,
            StepResult stepResult)
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
            FillTemplateString("Value", GetVariableValue(variable, rawStep));
            UpdateResultFlag(runtimeStatus.Result, runtimeStatus.FailedInfo);
            _uutPassed &= _stepPassed;
            _uutError |= _stepError;
            MoveToStepBuffer();
            _stepRunTicks += runtimeStatus.ExecutionTicks;
        }

        private void PrintStepInfo(RuntimeStatusData runtimeStatus, ISequenceStep rawStep, ISequenceStep runStep,
            bool retryEnabled)
        {
            // 对于类型为构造方法的Step，其结果不打印
            if (null != runStep.Function && runStep.Function.Type == FunctionType.Constructor)
            {
                return;
            }
            UpdateResultFlag(runtimeStatus.Result, runtimeStatus.FailedInfo);
            _uutPassed &= _stepPassed;
            _uutError |= _stepError;
            // 如果使能Retry，并且StepBuffer有数据，并且和上次名称相同，则无需写入数据
            if (retryEnabled && _stepBuffer.Length > 0 && _lastStepName.Equals(rawStep.Name))
            {
                return;
            }
            InitItemBuffer(_stepTemplate);
            FillTemplateString("SequenceName", rawStep.Name);
            MoveToStepBuffer();
        }
        
        private void UpdateResultFlag(StepResult stepResult, IFailedInfo failedInfo)
        {
            _stepPassed &= stepResult == StepResult.Pass || stepResult == StepResult.RetryFailed ||
                   stepResult == StepResult.Skip;
            _stepError |= stepResult == StepResult.Abort || stepResult == StepResult.Error ||
                         stepResult == StepResult.Timeout;
            if (null != failedInfo)
            {
                FailedType failedType = failedInfo.Type;
                _stepError |= failedType != FailedType.AssertionFailed && failedType != FailedType.RetryFailed;
                if (null == _failedInfo && failedType != FailedType.AssertionFailed)
                {
                    _failedInfo = failedInfo;
                }
            }
        }

        public override void EndPrint()
        {
            if (_stepBuffer.Length > 0)
            {
                FillStepBufferString("Result", GetCurrentStepResult());
                FillStepBufferString("ExecutionSeconds", (_stepRunTicks/1E7).ToString("F7"));
                _stepRunTicks = 0;
                _writeStream.WriteLine(_stepBuffer);
                _stepBuffer.Clear();
            }
//            DateTime time;
//            if (VariableValues.ContainsKey(Constants.StartTimeVar) &&
//                DateTime.TryParse(VariableValues[Constants.StartTimeVar], out time))
//            {
//                _startTime = time;
//            }
//            if (VariableValues.ContainsKey(Constants.EndTimeVar) &&
//                DateTime.TryParse(VariableValues[Constants.EndTimeVar], out time))
//            {
//                _endTime = time;
//            }
            _writeStream.WriteLine(_endTemplate);
            _writeStream.Flush();
            _writeStream.Close();
            // 如果串号非法，则说明当前测试无效，删除已写入的内容
            if (string.IsNullOrWhiteSpace(_serialNumber) || _serialNumber.Equals(Constants.NASerialNo) ||
                _serialNumber.Equals(NullValue))
            {
                _serialNumber = Constants.EmptySn;
            }
            string uutResult = GetUutResult();
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
            FillTemplateString("Model", ReportInfo.Model);
            string errorCode = _uutError && null != _failedInfo ? _failedInfo.ErrorCode.ToString() : "";
            FillTemplateString("ErrorCode", errorCode);

            _writeStream = new StreamWriter(_currentReportPath);
            _writeStream.BaseStream.SetLength(0);
            _writeStream.Flush();
            _writeStream.WriteLine(_itemBuffer);
            _writeStream.WriteLine(reportBody);
            _writeStream.Close();
            _writeStream.Dispose();
            _writeStream = null;
            ClearItemBuffer();
            string reportName = GetReportName(uutResult);
            int offset = 0;
            string fullPath = ReportDir + reportName;
            while (File.Exists(fullPath))
            {
                fullPath = $"{ReportDir}{reportName}_{offset}";
            }
            File.Move(_currentReportPath, fullPath);
            _currentReportPath = fullPath;
            _currentReport = _currentReportPath;
            _lastSerialNumber = _serialNumber;
            _currentReportPath = string.Empty;
            ClearParams();
            _uutIndex++;
            base.EndPrint();
        }

        private string GetReportName(string uutResult)
        {
            string reportName;
            int postfixIndex = 1;
            do
            {
                StringBuilder reportNameCache = new StringBuilder(ReportInfo.ReportNameFormat);
                reportNameCache.Replace("{SequenceName}", ReportInfo.SequenceName);
                reportNameCache.Replace("{SerialNumber}", $"[{_serialNumber}]");
                reportNameCache.Replace("{DUTIndex}", $"[{GetDutIndex()}]");
                reportNameCache.Replace("{Date}", _startTime.ToString("yyyyMMdd"));
                reportNameCache.Replace("{Time}", _startTime.ToString("HHmmss"));
                reportNameCache.Replace("{UUTResult}", uutResult);
                // 如果包含PostFix则替换
                if (ReportInfo.ReportNameFormat.Contains("{Postfix}"))
                {
                    reportNameCache.Replace("{Postfix}", postfixIndex++.ToString("D5"));
                }
                // 如果不包含PostFix，且文件已重复，则从后面添加索引
                else if (postfixIndex > 1)
                {
                    reportNameCache.Append("_").Append(postfixIndex - 1);
                }

                foreach (char invalidPathChar in InvalidPathChars)
                {
                    reportNameCache.Replace(invalidPathChar, InvalidCharReplacer);
                }
                reportName = reportNameCache.ToString();
            } while (File.Exists(reportName));
            return reportName;
        }

        private int GetDutIndex()
        {
            int dutIndex;
            if (VariableValues.ContainsKey(Constants.DutIndexVarName) && 
                int.TryParse(VariableValues[Constants.DutIndexVarName], out dutIndex))
            {
                return dutIndex;
            }
            return 0;
        }

        private string GetAvailableTemporaryPath()
        {
            const string fileNameFormat = "{0}uut{1}.txt";
            string rawPath = string.Format(fileNameFormat, ReportDir, _uutIndex);
            string path = rawPath;
            int index = 0;
            while (File.Exists(path))
            {
                path = rawPath + "_" + index++;
            }
            return path;
        }

        private string GetCurrentStepResult()
        {
            if (_stepError)
            {
                return "Error";
            }
            if (!_stepPassed)
            {
                return "Fail";
            }
            return _isActionStep ? "Done" : "Pass";
        }

        private string GetUutResult()
        {
            if (_uutError)
            {
                return "Error";
            }
            return _uutPassed ? "Pass" : "Fail";
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
                Logger.Print(LogLevel.WARN, $"Delete report {_currentReportPath} failed.{ex.Message}");
            }
        }
    }
}