namespace Linn.Purchasing.Resources.RequestResources
{
    public class UnacknowledgedOrdersRequestResource
    {
        public int? SupplierId { get; set; }
        
        public int? OrganisationId { get; set; }

        public string Name { get; set; }
    }
}
