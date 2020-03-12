namespace TestStation.Report
{
    public class ReportGlobalInfo
    {
        public string StationId { get; set; }
        public string SocketIndex { get; set; }
        public string Operator { get; set; }
        public string SequencePath { get; set; }
        public string SequenceName { get; set; }
        public string ReportNameFormat { get; set; }
        public string Model { get; set; }

        public ReportGlobalInfo()
        {
            StationId = "Station";
            SocketIndex = "0";
            Operator = "";
            SequencePath = string.Empty;
        }
    }
}