namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    public class WhatsInInspectionPurchaseOrdersViewModel
    {
        public string PartNumber { get; set; }

        public string State { get; set; }

        public string OrderType { get; set; }

        public int OrderNumber { get; set; }

        public decimal Qty { get; set; }

        public string Cancelled { get; set; }
    }
}
