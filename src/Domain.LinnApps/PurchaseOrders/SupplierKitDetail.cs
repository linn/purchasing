
namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class SupplierKitDetail
    {
        public SupplierKitDetail(Part part, decimal qty)
        {
            this.Part = part;
            this.Qty = qty;
        }

        public decimal Qty { get; set; }

        public Part Part { get; set; }
    }
}

