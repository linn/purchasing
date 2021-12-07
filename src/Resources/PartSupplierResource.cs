namespace Linn.Purchasing.Resources
{
    using Linn.Common.Resources;

    public class PartSupplierResource : HypermediaResource
    {
        public string PartNumber { get; set; }

        public string PartDescription { get; set; }

        public int SupplierId { get; set; }
    }
}
