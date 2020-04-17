using System;
using System.Collections.Generic;
using System.Text;
using TestFlow.SoftDevCommon;
using TestFlow.SoftDSevCommon;

namespace TestFlow.DevSoftware.Report
{
    public class ProductTestResult
    {
        public string Model { get; set; }
        public string SerialNo { get; set; }

        public Dictionary<string, ResultState> TestResult { get; set; }
        public Dictionary<string, string> ErrorInfo { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public double ElapsedTime { get; set; }

        public ResultState Result { get; set; }

        public ProductTestResult()
        {
            this.Model = Constants.NAModelNo;
            this.SerialNo = Constants.NASerialNo;
            this.TestResult = new Dictionary<string, ResultState>(10);
            this.ErrorInfo = new Dictionary<string, string>(10);
            this.Result = ResultState.NA;
            this.StartTime = DateTime.MinValue;
            this.EndTime = DateTime.MinValue;
        }

        public override string ToString()
        {
            const string timeformat = "yyyy-MM-dd HH:mm:ss.fff";
            StringBuilder strBuilder = new StringBuilder(2000);
            string newLine = Environment.NewLine;
            strBuilder.Append("Model: ").Append(Model).Append(newLine);
            strBuilder.Append("Serial No: ").Append(SerialNo).Append(newLine);
            strBuilder.Append("Start Time: ").Append(StartTime.ToString(timeformat)).Append(newLine);
            strBuilder.Append("End Time: ").Append(EndTime.ToString(timeformat)).Append(newLine);
            strBuilder.Append("Elapsed Time: ").Append(ElapsedTime.ToString("F3")).Append("s").Append(newLine);
            strBuilder.Append("Test Result: ").Append(Result).Append(newLine);
            if (null == TestResult || TestResult.Count == 0)
            {
                return strBuilder.ToString();
            }
            strBuilder.Append("Test Trace: ").Append(newLine);
            foreach (KeyValuePair<string, ResultState> keyValuePair in TestResult)
            {
                strBuilder.Append(keyValuePair.Key).Append('\t').Append(keyValuePair.Value);
                if (ErrorInfo.ContainsKey(keyValuePair.Key))
                {
                    strBuilder.Append('\t').Append(ErrorInfo[keyValuePair.Key]);
                }
                strBuilder.Append(newLine);
            }
            return strBuilder.ToString();
        }
    }
}