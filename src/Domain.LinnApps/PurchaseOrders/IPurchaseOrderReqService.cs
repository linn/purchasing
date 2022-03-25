namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    public interface IPurchaseOrderReqService
    {
        public void Update(PurchaseOrderReq entity, PurchaseOrderReq updatedEntity, IEnumerable<string> privileges);

        public PurchaseOrderReq Create(PurchaseOrderReq entity, IEnumerable<string> privileges);

        public void Cancel(PurchaseOrderReq entity, IEnumerable<string> privileges);

        void Authorise(PurchaseOrderReq entity, IEnumerable<string> privileges, int currentUserId);

        void FinanceApprove(PurchaseOrderReq entity, IEnumerable<string> privileges, int currentUserId);
    }
}
