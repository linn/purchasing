namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    using System;

    public class PartReceivedRecord
    {
        public string JobRef { get; set; }

        public string TqmsGroup { get; set; }

        public string PartNumber { get; set; }

        public decimal? OverstockQty { get; set; }

        public decimal? OverStockValue { get; set; }

        public string OrderNumber { get; set; }

        public decimal Qty { get; set; }

        public decimal PartPrice { get; set; }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public decimal? MaterialPrice { get; set; }

        public DateTime DateBooked { get; set; }
    }
}
