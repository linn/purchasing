namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    public class ReportOption
    {
        public ReportOption()
        {
        }

        public ReportOption(string option, string displayText, int? displaySequence = null)
        {
            this.Option = option;
            this.DisplayText = displayText;
            this.DisplaySequence = displaySequence;
        }

        public string Option { get; set; }

        public string DisplayText { get; set; }

        public int? DisplaySequence { get; set; }
    }
}
