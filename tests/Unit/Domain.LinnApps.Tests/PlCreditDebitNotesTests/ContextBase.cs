namespace Linn.Purchasing.Domain.LinnApps.Tests.PlCreditDebitNotesTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Email;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IPlCreditDebitNoteService Sut { get; private set; }

        protected IAuthorisationService MockAuthService { get; private set;  }

        protected IEmailService MockEmailService{ get; private set; }

        protected IRepository<Employee, int> EmployeeRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockAuthService = Substitute.For<IAuthorisationService>();
            this.MockEmailService = Substitute.For<IEmailService>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();

            this.Sut = new PlCreditDebitNoteService(
                this.MockAuthService, 
                this.MockEmailService, 
                this.EmployeeRepository);
        }
    }
}
