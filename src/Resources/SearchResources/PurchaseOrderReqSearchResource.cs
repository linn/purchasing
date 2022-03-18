namespace Linn.Purchasing.Resources.SearchResources
{
    public class PurchaseOrderReqSearchResource
    {
        public int ReqNumber { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public int SupplierId { get; set; }
    }
}
