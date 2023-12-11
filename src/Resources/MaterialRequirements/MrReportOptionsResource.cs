namespace Linn.Purchasing.Resources.MaterialRequirements
{
    using System.Collections.Generic;

    public class MrReportOptionsResource
    {
        public IEnumerable<ReportOptionResource> PartSelectorOptions { get; set; }

        public IEnumerable<ReportOptionResource> StockLevelOptions { get; set; }

        public IEnumerable<ReportOptionResource> OrderByOptions { get; set; }

        public IEnumerable<ReportOptionResource> PartOptions { get; set; }

        public IEnumerable<MrpRunLogResource> AvailableJobRefs { get; set; }
    }
}
