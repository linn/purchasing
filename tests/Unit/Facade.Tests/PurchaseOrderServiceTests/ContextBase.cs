﻿namespace Linn.Purchasing.Facade.Tests.PurchaseOrderServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IAuthorisationService AuthService { get; private set; }

        protected IBuilder<PurchaseOrder> Builder { get; private set; }

        protected IPurchaseOrderService DomainService { get; private set; }

        protected LinnDeliveryAddressResourceBuilder LinnDeliveryAddressResourceBuilder { get; private set; }

        protected IRepository<OverbookAllowedByLog, int> OverbookAllowedByLogRepository { get; private set; }

        protected PurchaseOrderDeliveryResourceBuilder PurchaseOrderDeliveryResourceBuilder { get; private set; }

        protected PurchaseOrderDetailResourceBuilder PurchaseOrderDetailResourceBuilder { get; private set; }

        protected PurchaseOrderPostingResourceBuilder PurchaseOrderPostingResourceBuilder { get; private set; }

        protected AddressResourceBuilder AddressResourceBuilder { get; private set; }

        protected SupplierResourceBuilder SupplierResourceBuilder { get; private set; }

        protected IRepository<PurchaseOrder, int> PurchaseOrderRepository { get; private set; }

        protected IRepository<FullAddress, int> FullAddressRepository { get; private set; }

        protected IRepository<NominalAccount, int> MockNominalAccountRepository { get; private set; }

        protected PurchaseOrderFacadeService Sut { get; private set; }

        protected ITransactionManager TransactionManager { get; private set; }

        protected ILog Logger { get; private set; }

        protected IRepository<Supplier, int> SupplierRepository { get; private set; }

        protected IPlCreditDebitNoteService PlCreditDebitNoteService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.PurchaseOrderRepository = Substitute.For<IRepository<PurchaseOrder, int>>();
            this.FullAddressRepository = Substitute.For<IRepository<FullAddress, int>>();
            this.MockNominalAccountRepository = Substitute.For<IRepository<NominalAccount, int>>();
            this.OverbookAllowedByLogRepository = Substitute.For<IRepository<OverbookAllowedByLog, int>>();
            this.SupplierRepository = Substitute.For<IRepository<Supplier, int>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.DomainService = Substitute.For<IPurchaseOrderService>();
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.PurchaseOrderDeliveryResourceBuilder = new PurchaseOrderDeliveryResourceBuilder();
            this.PurchaseOrderPostingResourceBuilder = new PurchaseOrderPostingResourceBuilder(this.MockNominalAccountRepository);
            this.LinnDeliveryAddressResourceBuilder = new LinnDeliveryAddressResourceBuilder();
            this.AddressResourceBuilder = new AddressResourceBuilder(this.FullAddressRepository);
            this.SupplierResourceBuilder = new SupplierResourceBuilder(
                this.AuthService,
                new SupplierContactResourceBuilder(),
                this.AddressResourceBuilder);
            this.PurchaseOrderDetailResourceBuilder = new PurchaseOrderDetailResourceBuilder(
                this.PurchaseOrderDeliveryResourceBuilder,
                this.PurchaseOrderPostingResourceBuilder,
                this.AuthService);

            this.Builder = new PurchaseOrderResourceBuilder(
                this.AuthService,
                this.PurchaseOrderDetailResourceBuilder,
                this.LinnDeliveryAddressResourceBuilder,
                this.AddressResourceBuilder,
                this.SupplierResourceBuilder);

            this.Logger = Substitute.For<ILog>();

            this.PlCreditDebitNoteService = Substitute.For<IPlCreditDebitNoteService>();

            this.Sut = new PurchaseOrderFacadeService(
                this.PurchaseOrderRepository,
                this.TransactionManager,
                this.Builder,
                this.DomainService,
                this.OverbookAllowedByLogRepository,
                this.PlCreditDebitNoteService,
                this.Logger);
        }
    }
}
