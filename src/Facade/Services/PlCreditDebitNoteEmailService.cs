namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.IO;
    using System.Linq;

    using Linn.Common.Email;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers.Exceptions;
    using Linn.Purchasing.Resources;

    public class PlCreditDebitNoteEmailService : IPlCreditDebitNoteEmailService
    {
        private readonly IEmailService emailService;

        private readonly IRepository<PlCreditDebitNote, int> noteRepository;

        public PlCreditDebitNoteEmailService(
            IEmailService emailService,
            IRepository<PlCreditDebitNote, int> repository)
        {
            this.noteRepository = repository;
            this.emailService = emailService;
        }

        public IResult<ProcessResultResource> SendEmail(int noteNumber, Stream attachment)
        {
            var note = this.noteRepository.FindById(noteNumber);
            var contact = note.Supplier.SupplierContacts.FirstOrDefault(c => c.IsMainOrderContact == "Y");

            if (contact == null)
            {
                return new SuccessResult<ProcessResultResource>(new ProcessResultResource
                                                                    {
                                                                        Success = false,
                                                                        Message = "Supplier has no main order contact"
                                                                    });
            }

            try
            {
                this.emailService.SendEmail(
                    contact.EmailAddress, 
                    $"{contact.Person.FirstName} {contact.Person.LastName}",
                    null, 
                    null,
                    "purchasing@linn.co.uk", 
                    "Linn",
                    $"Note {noteNumber}",
                    string.Empty,
                    attachment,
                    noteNumber.ToString());

                return new SuccessResult<ProcessResultResource>(new ProcessResultResource(true, "Email Requested"));
            }
            catch (Exception e)
            {
                return new SuccessResult<ProcessResultResource>(
                    new ProcessResultResource { Success = false, Message = e.Message });
            }
        }
    }
}
