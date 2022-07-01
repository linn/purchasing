namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System.Collections.Generic;

    public class MrReport
    {
        public string JobRef { get; set; }

        public int RunWeekNumber { get; set; }

        public IEnumerable<MrHeader> Headers { get; set; }

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
    }
}
