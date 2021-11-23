namespace Linn.Purchasing.Resources
{
    using System.Collections.Generic;

    using Linn.Common.Resources;

    public class ThingResource : HypermediaResource
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string RecipientAddress { get; set; }

        public string RecipientName { get; set; }

        public ThingCodeResource Code { get; set; }

        public IEnumerable<ThingDetailResource> Details { get; set; }
    }
}
