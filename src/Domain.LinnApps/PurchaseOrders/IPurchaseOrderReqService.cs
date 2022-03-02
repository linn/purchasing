namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    public interface IPurchaseOrderReqService
    {
        public bool Update(PurchaseOrderReq entity, PurchaseOrderReq updatedEntity, IEnumerable<string> privileges);

        public PurchaseOrderReq Create(PurchaseOrderReq entity, IEnumerable<string> privileges);
    }
}
