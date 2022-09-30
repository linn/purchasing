﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PlCreditDebitNotesTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Email;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IPlCreditDebitNoteService Sut { get; private set; }

        protected IAuthorisationService MockAuthService { get; private set;  }

        protected IEmailService MockEmailService { get; private set; }

        protected IRepository<Employee, int> MockEmployeeRepository { get; private set; }

        protected IRepository<PlCreditDebitNote, int> MockRepository { get; private set; }

        protected IRepository<Supplier, int> MockSupplierRepository { get; private set; }

        protected IDatabaseService MockDatabaseService { get; private set; }

        protected ISalesTaxPack MockSalesTaxPack { get; private set; }

        protected IRepository<CreditDebitNoteType, string> MockNoteTypeRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockAuthService = Substitute.For<IAuthorisationService>();
            this.MockEmailService = Substitute.For<IEmailService>();
            this.MockEmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.MockNoteTypeRepository = Substitute.For<IRepository<CreditDebitNoteType, string>>();
            this.Sut = new PlCreditDebitNoteService(
                this.MockAuthService, 
                this.MockEmailService, 
                this.MockEmployeeRepository,
                this.MockRepository,
                this.MockSalesTaxPack,
                this.MockSupplierRepository,
                this.MockDatabaseService,
                this.MockNoteTypeRepository);
        }
    }
}
