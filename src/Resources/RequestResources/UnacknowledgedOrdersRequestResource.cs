namespace Linn.Purchasing.Resources.RequestResources
{
    public class UnacknowledgedOrdersRequestResource
    {
        public int? SupplierId { get; set; }
        
        public int? SupplierGroupId { get; set; }

        public string Name { get; set; }
    }
}
