namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Email;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;

    public class PlCreditDebitNoteService : IPlCreditDebitNoteService
    {
        private readonly IAuthorisationService authService;

        private readonly IEmailService emailService;

        private readonly IRepository<Employee, int> employeeRepository;

        public PlCreditDebitNoteService(
            IAuthorisationService authService,
            IEmailService emailService,
            IRepository<Employee, int> employeeRepository)
        {
            this.authService = authService;
            this.emailService = emailService;
            this.employeeRepository = employeeRepository;
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

        public ProcessResult SendEmails(
            Employee sender, 
            PlCreditDebitNote note, 
            Stream pdfAttachment)
        {
            var contact = note.Supplier?.SupplierContacts?.FirstOrDefault(c => c.IsMainOrderContact == "Y");

            if (contact == null)
            {
                return new ProcessResult 
                           {
                               Success = false,
                               Message = "Supplier has no main order contact"
                           };
            }

            if (string.IsNullOrEmpty(sender?.PhoneListEntry?.EmailAddress))
            {
                return new ProcessResult
                           {
                               Success = false,
                               Message = "Cannot find sender email address"
                           };
            }

            var bccFinancePersonId = note.Supplier.Country.Equals("GB") ? 33039 : 6001;

            var bccFinancePerson = this.employeeRepository.FindById(bccFinancePersonId);

            var bccEntry = new Dictionary<string, string>
                               {
                                   { "name", bccFinancePerson.FullName},
                                   { "address", bccFinancePerson.PhoneListEntry.EmailAddress }
                               };

            var bccList = new List<Dictionary<string, string>>
                              {
                                  bccEntry
                              };

            try
            {
                this.emailService.SendEmail(
                    contact.EmailAddress.Trim(),
                    $"{contact.Person.FirstName} {contact.Person.LastName}",
                    null,
                    bccList,
                    sender.PhoneListEntry.EmailAddress.Trim(),
                    sender.FullName,
                    $"Linn Products {note.NoteType.PrintDescription} {note.NoteNumber}",
                    $"Attached is a copy of Linn Products {note.NoteType.PrintDescription} {note.NoteNumber}",
                    pdfAttachment,
                    $"{note.NoteType.PrintDescription} {note.NoteNumber}");

                return new ProcessResult(true, "Email Sent");
            }
            catch (Exception e)
            {
                return new ProcessResult
                           {
                               Success = false,
                               Message = $"Error sending email. Error Message: {e.Message}"
                           };
            }
        }
    }
}
