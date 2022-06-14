namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Keys;

    public interface IPurchaseOrderDeliveryRepository : IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey>
    {
        IEnumerable<PurchaseOrderDelivery> SearchByOrderWithWildcard(string expression);
    }
}
