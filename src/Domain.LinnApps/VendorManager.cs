namespace Linn.Purchasing.Domain.LinnApps
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    public class VendorManager
    {
        public string VmId { get; set; }

        public int UserNumber { get; set; }

        public string PmMeasured { get; set; }

        public Employee Employee { get; }
    }
}
