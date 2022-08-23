namespace Linn.Purchasing.Resources.RequestResources
{
    using System.Collections.Generic;

    public class PurchaseOrdersProcessRequestResource
    {
        public IEnumerable<int> Orders { get; set; }

        public string CopySelf { get; set; } = "false";
    }
}
