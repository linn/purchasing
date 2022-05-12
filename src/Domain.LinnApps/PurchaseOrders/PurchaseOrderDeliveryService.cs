namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Keys;

    public class PurchaseOrderDeliveryService : IPurchaseOrderDeliveryService
    {
        private readonly IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> repository;

        public PurchaseOrderDeliveryService(IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<PurchaseOrderDelivery> SearchDeliveries(
            string supplierSearchTerm, string orderNumberSearchTerm, bool includeAcknowledged)
        {
            var result = this.repository.FindAll();
            if (!string.IsNullOrEmpty(supplierSearchTerm))
            {
                if (int.TryParse(supplierSearchTerm, out var supplierId))
                {
                    result = result.Where(x => x.PurchaseOrderDetail.PurchaseOrder.SupplierId == supplierId);
                }
                else
                {
                    result = result.Where(x => 
                        x.PurchaseOrderDetail.PurchaseOrder.Supplier.Name.Contains(supplierSearchTerm.ToUpper()));
                }
            }

            if (!string.IsNullOrEmpty(orderNumberSearchTerm))
            {
                result = result.Where(x => x.OrderNumber.ToString().Contains(orderNumberSearchTerm));
            }

            if (!includeAcknowledged)
            {
                result = result.Where(x => !x.DateAdvised.HasValue);
            }

            return result.OrderBy(x => x.OrderNumber);
        }
    }
}
