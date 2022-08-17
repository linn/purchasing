namespace Linn.Purchasing.Resources.RequestResources
{
    public class DeliveryPerformanceRequestResource
    {
        public int StartPeriod { get; set; }
        
        public int EndPeriod { get; set; }
        
        public int? SupplierId { get; set; }

        public string VendorManager { get; set; }
    }
}
