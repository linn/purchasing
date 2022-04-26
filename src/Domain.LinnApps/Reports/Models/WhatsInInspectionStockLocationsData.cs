namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    using System;

    public class WhatsInInspectionStockLocationsData
    {
        public string PartNumber { get; set; }

        public string State { get; set; }

        public string Batch { get; set; }

        public string Location { get; set; }

        public decimal Qty { get; set; }

        public DateTime? StockRotationDate { get; set; }

        public string BatchRef { get; set; }
    }
}
