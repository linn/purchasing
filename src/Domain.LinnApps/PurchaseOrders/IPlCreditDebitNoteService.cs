namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    public interface IPlCreditDebitNoteService
    {
        public void CloseDebitNote(
            PlCreditDebitNote toClose, 
            string reason,
            int closedBy,
            IEnumerable<string> privileges);

        public void CancelDebitNote(
            PlCreditDebitNote toCancel,
            string reason,
            int cancelledBy,
            IEnumerable<string> privileges);

        public void UpdatePlCreditDebitNote(
            PlCreditDebitNote current,
            PlCreditDebitNote updated,
            IEnumerable<string> privileges);
    }
}
