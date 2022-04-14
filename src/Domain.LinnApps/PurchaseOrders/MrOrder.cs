namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    public class MrOrder
    {
        public string JobRef { get; set; }

        public int LineNumber { get; set; }

        public string PartNumber { get; set; }

        public int OrderNumber { get; set; }
    }
}
