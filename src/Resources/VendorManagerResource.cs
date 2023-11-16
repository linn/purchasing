namespace Linn.Purchasing.Resources
{
    using Linn.Common.Resources;

    public class VendorManagerResource : HypermediaResource
    {
        public string VmId { get; set; }

        public int UserNumber { get; set; }

        public string Name { get; set; }
    }
}
