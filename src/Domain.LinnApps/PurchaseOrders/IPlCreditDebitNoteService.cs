namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;
    using System.IO;

    public interface IPlCreditDebitNoteService
    {
        void CloseDebitNote(
            PlCreditDebitNote toClose, 
            string reason,
            int closedBy,
            IEnumerable<string> privileges);

        void CancelDebitNote(
            PlCreditDebitNote toCancel,
            string reason,
            int cancelledBy,
            IEnumerable<string> privileges);

        void UpdatePlCreditDebitNote(
            PlCreditDebitNote current,
            PlCreditDebitNote updated,
            IEnumerable<string> privileges);

        ProcessResult SendEmails(
            Employee sender,
            PlCreditDebitNote note, 
            Stream pdfAttachment);

        void CreateDebitOrCreditNoteFromPurchaseOrder(PurchaseOrder order);

        PlCreditDebitNote CreateNote(PlCreditDebitNote candidate, IEnumerable<string> privileges);
    }
}
