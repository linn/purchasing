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

        public PlCreditDebitNote CloseDebitNote(
            PlCreditDebitNote toClose, 
            string reason, 
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PlCreditDebitNoteClose, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to close debit notes");
            }

            toClose.DateClosed = DateTime.Today;
            toClose.ReasonClosed = reason;

            return toClose;
        }
    }
}
