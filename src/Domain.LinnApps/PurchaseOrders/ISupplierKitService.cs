namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    public interface ISupplierKitService
    {
        IEnumerable<SupplierKit> GetSupplierKits(PurchaseOrder order, bool getComponents = false);
    }
}
