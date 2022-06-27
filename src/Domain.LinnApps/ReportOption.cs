namespace Linn.Purchasing.Domain.LinnApps
{
    public class ReportOption
    {
        public ReportOption()
        {
        }

        public ReportOption(string option, string displayText, int? displaySequence = null, string dataTag = null)
        {
            this.Option = option;
            this.DisplayText = displayText;
            this.DisplaySequence = displaySequence;
            this.DataTag = dataTag;
        }

        public string Option { get; set; }

        public string DisplayText { get; set; }

        public int? DisplaySequence { get; set; }

        public string DataTag { get; set; }

        public bool? DefaultOption { get; set; }
    }
}
