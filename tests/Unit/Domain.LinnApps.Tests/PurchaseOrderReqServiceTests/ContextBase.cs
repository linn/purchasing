﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Email;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IEmailService EmailService { get; private set; }

        protected IRepository<Employee, int> EmployeeRepository { get; private set; }

        protected IAuthorisationService MockAuthService { get; private set; }

        protected ICurrencyPack MockCurrencyPack { get; private set; }

        protected IPurchaseOrderAutoOrderPack MockPurchaseOrderAutoOrderPack { get; private set; }

        protected IPurchaseOrderReqsPack MockPurchaseOrderReqsPack { get; private set; }

        protected IPurchaseOrdersPack MockPurchaseOrdersPack { get; private set; }

        protected IRepository<PurchaseOrderReqStateChange, PurchaseOrderReqStateChangeKey> MockReqsStateChangeRepository
        {
            get;
            private set;
        }

        protected IPurchaseOrderReqService Sut { get; private set; }

        protected IQueryRepository<Part> MockPartRepository { get; private set; }

        protected IRepository<Supplier, int> MockSupplierRepository { get; private set; }

        protected IRepository<NominalAccount, int> NominalAccountRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockAuthService = Substitute.For<IAuthorisationService>();
            this.MockPurchaseOrderReqsPack = Substitute.For<IPurchaseOrderReqsPack>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.EmailService = Substitute.For<IEmailService>();
            this.MockReqsStateChangeRepository =
                Substitute.For<IRepository<PurchaseOrderReqStateChange, PurchaseOrderReqStateChangeKey>>();
            this.MockPurchaseOrderAutoOrderPack = Substitute.For<IPurchaseOrderAutoOrderPack>();
            this.MockPurchaseOrdersPack = Substitute.For<IPurchaseOrdersPack>();
            this.MockCurrencyPack = Substitute.For<ICurrencyPack>();
            this.MockSupplierRepository = Substitute.For<IRepository<Supplier, int>>();
            this.MockPartRepository = Substitute.For<IQueryRepository<Part>>();
            this.NominalAccountRepository = Substitute.For<IRepository<NominalAccount, int>>();

            this.Sut = new PurchaseOrderReqService(
                "app.linn",
                this.MockAuthService,
                this.MockPurchaseOrderReqsPack,
                this.EmployeeRepository,
                this.EmailService,
                this.MockReqsStateChangeRepository,
                this.MockPurchaseOrderAutoOrderPack,
                this.MockPurchaseOrdersPack,
                this.MockCurrencyPack,
                this.MockPartRepository,
                this.MockSupplierRepository,
                this.NominalAccountRepository);
        }
    }
}
