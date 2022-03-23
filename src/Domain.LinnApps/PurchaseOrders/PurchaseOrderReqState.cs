namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    public class PurchaseOrderReqState
    {
        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public string IsFinalState { get; set; }

        public string State { get; set; }
    }
}
