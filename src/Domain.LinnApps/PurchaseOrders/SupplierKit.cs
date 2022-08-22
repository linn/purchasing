namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class SupplierKit : SupplierKitDetail
    {
        public SupplierKit(Part part, decimal qty) : base(part, qty)
        {
        }

        public SupplierKit(Part part, decimal qty, IEnumerable<SupplierKitDetail> details) : base(part, qty)
        {
            this.Details = details;
        }

        public IEnumerable<SupplierKitDetail> Details { get; set; }
    }
}
