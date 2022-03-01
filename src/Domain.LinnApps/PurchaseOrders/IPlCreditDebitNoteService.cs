namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    public interface IPlCreditDebitNoteService
    {
        public PlCreditDebitNote CloseDebitNote(
            PlCreditDebitNote toClose, 
            string reason,
            IEnumerable<string> privileges);
    }
}
