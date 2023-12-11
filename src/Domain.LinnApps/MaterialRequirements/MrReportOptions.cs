namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System.Collections.Generic;

    public class MrReportOptions
    {
        public IEnumerable<ReportOption> PartSelectorOptions { get; set; }

        public IEnumerable<ReportOption> StockLevelOptions { get; set; }

        public IEnumerable<ReportOption> OrderByOptions { get; set; }

        public IEnumerable<ReportOption> PartOptions { get; set; }

        public IEnumerable<MrpRunLog> AvailableJobRefs { get; set; }
    }
}
