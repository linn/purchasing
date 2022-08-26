namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    public class PlReceipt
    {
        public int Id { get; set; }

        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public int DeliverySeq { get; set; }

        public decimal Qty { get; set; }
    }
}

