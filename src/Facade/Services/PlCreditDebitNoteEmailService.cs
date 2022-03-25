namespace Linn.Purchasing.Facade.Services
{
    using System.IO;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class PlCreditDebitNoteEmailService : IPlCreditDebitNoteEmailService
    {
        private readonly IPlCreditDebitNoteService domainService;

        private readonly IRepository<PlCreditDebitNote, int> noteRepository;

        private readonly IRepository<Employee, int> employeeRepository;

        public PlCreditDebitNoteEmailService(
            IPlCreditDebitNoteService domainService,
            IRepository<Employee, int> employeeRepository,
            IRepository<PlCreditDebitNote, int> repository)
        {
            this.domainService = domainService;
            this.noteRepository = repository;
            this.employeeRepository = employeeRepository;
        }

        public IResult<ProcessResultResource> SendEmail(
            int senderUserNumber, int noteNumber, Stream attachment)
        {
            var note = this.noteRepository.FindById(noteNumber);
            var sender = this.employeeRepository.FindById(senderUserNumber);

            var result = this.domainService.SendEmails(sender, note, attachment);

            return new SuccessResult<ProcessResultResource>(new ProcessResultResource(result.Success, result.Message));
        }
    }
}
