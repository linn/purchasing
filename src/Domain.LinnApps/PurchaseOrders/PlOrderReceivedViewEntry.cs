namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    public class PlOrderReceivedViewEntry
    {
        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public decimal QtyOutstanding { get; set; }
    }
}
