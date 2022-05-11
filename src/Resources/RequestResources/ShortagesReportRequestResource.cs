namespace Linn.Purchasing.Resources.RequestResources
{
    public class ShortagesReportRequestResource
    {
        public int? PurchaseLevel { get; set; }

        public int? Supplier { get; set; }

        public string VendorManager { get; set; }
    }
}