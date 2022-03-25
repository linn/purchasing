namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    public class PurchaseOrderReqStateChange
    {
        public string ComputerAllowed { get; set; }

        public string FromState { get; set; }

        public string ToState { get; set; }

        public string UserAllowed { get; set; }
    }
}
