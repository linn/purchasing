namespace Linn.Purchasing.Resources.MaterialRequirements
{
    using System.Collections.Generic;

    public class MrRequestResource
    {
        public string JobRef { get; set; }

        public string TypeOfReport { get; set; }

        public string PartSelector { get; set; }

        public string StockLevelSelector { get; set; }

        public string PartOption { get; set; }

        public string OrderBySelector { get; set; }

        public IEnumerable<string> PartNumbers { get; set; }

        public string PartNumber { get; set; }

        public int? SupplierId { get; set; }

        public string PartNumberList { get; set; }

        public string StockCategoryName { get; set; }

        public int ReportChunk { get; set; }
    }
}
