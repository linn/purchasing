namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;
    using System.IO;

    public interface IPurchaseOrderReqService
    {
        void Update(PurchaseOrderReq entity, PurchaseOrderReq updatedEntity, IEnumerable<string> privileges);

        PurchaseOrderReq Create(PurchaseOrderReq entity, IEnumerable<string> privileges);

        void Cancel(PurchaseOrderReq entity, IEnumerable<string> privileges);

        void Authorise(PurchaseOrderReq entity, IEnumerable<string> privileges, int currentUserId);

        void FinanceApprove(PurchaseOrderReq entity, IEnumerable<string> privileges, int currentUserId);

        ProcessResult SendEmails(
            int sender,
            string to,
            int reqNumber,
            Stream pdfAttachment);
    }
}
