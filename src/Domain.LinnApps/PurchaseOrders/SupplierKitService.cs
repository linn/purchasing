namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class SupplierKitService : ISupplierKitService
    {
        private readonly IBomDetailRepository bomDetailRepository;

        public SupplierKitService(IBomDetailRepository bomDetailRepository)
        {
            this.bomDetailRepository = bomDetailRepository;
        }

        private SupplierKit AddComponents(Part part, decimal qty)
        {
            var bom = this.bomDetailRepository.GetLiveBomDetails(part.PartNumber).ToList();
            var components = bom.Select(d => new SupplierKitDetail(d.Part, qty * d.Qty));
            return new SupplierKit(part, qty, components);
        }

        public IEnumerable<SupplierKit> GetSupplierKits(PurchaseOrder order, bool getComponents = false)
        {
            if (!getComponents)
            {
                return order.Details.Where(d => d.Part.SupplierAssembly()).Select(d => new SupplierKit(d.Part, (decimal)d.OurQty));
            }

            return order.Details.Where(d => d.Part.SupplierAssembly())
                .Select(d => this.AddComponents(d.Part, (decimal) d.OurQty));
        }
    }
}
