namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    public class MonthlyForecastPart
    {
        public string MrPartNumber { get; set; }

        public string SupplierDesignation { get; set; }

        public decimal StartingQty { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal MinimumOrderQty { get; set; }

        public int PreferredSupplier { get; set; }

        public decimal TotalNettReqtValue { get; set; }

        public int LeadTimeWeek { get; set; }
    }
}
