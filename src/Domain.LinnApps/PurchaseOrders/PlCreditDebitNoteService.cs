namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Authorisation;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;

    public class PlCreditDebitNoteService : IPlCreditDebitNoteService
    {
        private readonly IAuthorisationService authService;

        public PlCreditDebitNoteService(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public void CloseDebitNote(
            PlCreditDebitNote toClose, 
            string reason,
            int closedBy,
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PlCreditDebitNoteClose, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to close debit notes");
            }

            toClose.DateClosed = DateTime.Today;
            toClose.ReasonClosed = reason;
            toClose.ClosedBy = closedBy;
        }

        public void CancelDebitNote(
            PlCreditDebitNote toCancel,
            string reason, 
            int cancelledBy, 
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PlCreditDebitNoteCancel, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to cancel debit notes");
            }

            toCancel.DateCancelled = DateTime.Today;
            toCancel.ReasonCancelled = reason;
            toCancel.CancelledBy = cancelledBy;
        }

        public void UpdatePlCreditDebitNote(
            PlCreditDebitNote current, PlCreditDebitNote updated, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PlCreditDebitNoteUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update credit/debit notes");
            }

            current.Notes = updated.Notes;
        }
    }
}
