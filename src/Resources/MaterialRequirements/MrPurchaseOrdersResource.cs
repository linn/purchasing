namespace Linn.Purchasing.Resources.MaterialRequirements
{
    using System.Collections.Generic;

    public class MrPurchaseOrdersResource
    {
        public IEnumerable<MrPurchaseOrderResource> Orders { get; set; }
    }
}
