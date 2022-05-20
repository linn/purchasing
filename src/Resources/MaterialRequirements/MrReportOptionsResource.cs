namespace Linn.Purchasing.Resources.MaterialRequirements
{
    using System.Collections.Generic;

    public class MrReportOptionsResource
    {
        public IEnumerable<ReportOptionResource> PartSelectorOptions { get; set; }

        public IEnumerable<ReportOptionResource> DangerLevelOptions { get; set; }
    }
}
