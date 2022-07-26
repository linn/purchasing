namespace Linn.Purchasing.Resources
{
    using System.Collections.Generic;

    using Linn.Common.Resources;

    public class AutomaticPurchaseOrderResource : HypermediaResource
    {
        public int Id { get; set; }

        public int StartedBy { get; set; }

        public string JobRef { get; set; }

        public string DateRaised { get; set; }

        public int? SupplierId { get; set; }

        public int? Planner { get; set; }

        public IEnumerable<AutomaticPurchaseOrderDetailResource> Details { get; set; }
    }
}
