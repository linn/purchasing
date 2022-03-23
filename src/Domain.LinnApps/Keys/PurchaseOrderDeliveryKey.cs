namespace Linn.Purchasing.Domain.LinnApps.Keys
{
    public class PurchaseOrderDeliveryKey
    {
        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public int DeliverySequence { get; set; }
    }
}
