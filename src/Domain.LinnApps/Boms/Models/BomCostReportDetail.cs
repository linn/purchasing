namespace Linn.Purchasing.Domain.LinnApps.Boms.Models
{
    public class BomCostReportDetail 
    {
        public int DetailId { get; set; }

        public string PartNumber { get; set; }

        public string PartDescription { get; set; }

        public string BomType { get; set; }

        public int? PreferredSupplier { get; set; }

        public int? LeadTime { get; set; }

        public decimal? Qty { get; set; }

        public decimal? StandardPrice { get; set; }

        public decimal? MaterialPrice { get; set; }

        public decimal? LabourTimeMins { get; set; }

        public string BomName { get; set; }
    }
}
