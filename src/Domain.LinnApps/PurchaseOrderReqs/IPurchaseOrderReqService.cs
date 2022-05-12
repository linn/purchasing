namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs
{
    using System.Collections.Generic;
    using System.IO;

    public interface IPurchaseOrderReqService
    {
        void Update(PurchaseOrderReq entity, PurchaseOrderReq updatedEntity, IEnumerable<string> privileges);

        void CreateOrderFromReq(PurchaseOrderReq entity, IEnumerable<string> privileges, int currentUserId);

        PurchaseOrderReq Create(PurchaseOrderReq entity, IEnumerable<string> privileges);

        void Cancel(PurchaseOrderReq entity, IEnumerable<string> privileges);

        void Authorise(PurchaseOrderReq entity, IEnumerable<string> privileges, int currentUserId);

        void FinanceApprove(PurchaseOrderReq entity, IEnumerable<string> privileges, int currentUserId);

        ProcessResult SendEmails(
            int sender,
            string to,
            int reqNumber,
            Stream pdfAttachment);

        ProcessResult SendAuthorisationRequestEmail(int currentUser, int toEmp, PurchaseOrderReq req);

        ProcessResult SendFinanceCheckRequestEmail(int currentUser, int toEmp, PurchaseOrderReq req);
    }
}
