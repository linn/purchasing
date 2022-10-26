namespace Linn.Purchasing.Resources.MaterialRequirements
{
    using System.Collections.Generic;

    public class MrReportResource
    {
        public string JobRef { get; set; }

        public string RunDate { get; set; }

        public int ReportChunk { get; set; }

        public int TotalChunks { get; set; }

        public string PartSelectorOption { get; set; }

        public string StockLevelOption { get; set; }

        public string PartOption { get; set; }

        public string OrderByOption { get; set; }

        public int? SupplierIdOption { get; set; }

        public IEnumerable<string> PartNumbersOption { get; set; }

        public string PartNumberListOption { get; set; }

        public string StockCategoryNameOption { get; set; }
        
        public int? MinimumLeadTimeWeeks { get; set; }

        public int? MinimumAnnualUsage { get; set; }

        public string RunDateOption { get; set; }

        public IEnumerable<MrHeaderResource> Results { get; set; }
    }
}
