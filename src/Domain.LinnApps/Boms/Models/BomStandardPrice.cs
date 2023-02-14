namespace Linn.Purchasing.Domain.LinnApps.Boms.Models
{
    public class BomStandardPrice
    {
        public int? Depth { get; set; }

        public string BomName { get; set; }

        public decimal? MaterialPrice { get; set; }

        public decimal? StandardPrice { get; set; }

        public decimal? StockMaterialVariance { get; set; }

        public decimal? LoanMaterialVariance { get; set; }

        public int? AllocLines { get; set; }
    }
}
