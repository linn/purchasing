namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Email;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IPurchaseOrderReqService Sut { get; private set; }

        protected IAuthorisationService MockAuthService { get; private set; }

        protected IPurchaseOrderReqsPack MockPurchaseOrderReqsPack { get; private set; }

        protected IRepository<Employee, int> EmployeeRepository { get; private set; }

        protected IEmailService EmailService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockAuthService = Substitute.For<IAuthorisationService>();
            this.MockPurchaseOrderReqsPack = Substitute.For<IPurchaseOrderReqsPack>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.EmailService = Substitute.For<IEmailService>();

            this.Sut = new PurchaseOrderReqService(
                this.MockAuthService,
                this.MockPurchaseOrderReqsPack,
                this.EmployeeRepository,
                this.EmailService);
        }
    }
}
