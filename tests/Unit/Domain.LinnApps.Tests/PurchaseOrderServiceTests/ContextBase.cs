namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Email;
    using Linn.Common.Logging;
    using Linn.Common.Pdf;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IEmailService EmailService { get; private set; }

        protected IRepository<Employee, int> EmployeeRepository { get; private set; }

        protected IRepository<MiniOrder, int> MiniOrderRepository { get; private set; }

        protected IAuthorisationService MockAuthService { get; private set; }

        protected IDatabaseService MockDatabaseService { get; private set; }

        protected IPdfService PdfService { get; private set; }

        protected ISupplierKitService SupplierKitService { get; private set; }

        protected IPurchaseLedgerPack PurchaseLedgerPack { get; private set; }

        protected IPurchaseOrdersPack PurchaseOrdersPack { get; private set; }

        protected ICurrencyPack CurrencyPack { get; private set; }

        protected IPurchaseOrderService Sut { get; private set; }

        protected IRepository<Supplier, int> SupplierRepository { get; private set; }

        protected IRepository<LinnDeliveryAddress, int> LinnDeliveryAddressRepository { get; private set; }

        protected IRepository<PurchaseOrder, int> PurchaseOrderRepository { get; private set; }

        protected IHtmlTemplateService<PurchaseOrder> HtmlTemplateService { get; private set; }

        protected ISingleRecordRepository<PurchaseLedgerMaster> PurchaseLedgerMaster { get; private set; }

        protected IRepository<NominalAccount, int> NominalAccountRepository { get; private set; }

        protected IRepository<PartSupplier, PartSupplierKey> PartSupplierRepository { get; private set; }

        protected IQueryRepository<Part> PartQueryRepository { get; private set; }

        protected ILog Log { get; private set; }

        protected IHtmlTemplateService<PlCreditDebitNote> NoteTemplateService { get; private set; }

        protected IRepository<PlCreditDebitNote, int> NoteRepository { get; private set; }

        protected IQueryRepository<PlOrderReceivedViewEntry> OrderReceivedView { get; private set; }

        protected IRepository<CancelledOrderDetail, int> CancelledOrderDetailRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockAuthService = Substitute.For<IAuthorisationService>();
            this.MockDatabaseService = Substitute.For<IDatabaseService>();
            this.PurchaseLedgerPack = Substitute.For<IPurchaseLedgerPack>();
            this.PdfService = Substitute.For<IPdfService>();
            this.EmailService = Substitute.For<IEmailService>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.MiniOrderRepository = Substitute.For<IRepository<MiniOrder, int>>();
            this.SupplierKitService = Substitute.For<ISupplierKitService>();
            this.SupplierRepository = Substitute.For<IRepository<Supplier, int>>();
            this.LinnDeliveryAddressRepository = Substitute.For<IRepository<LinnDeliveryAddress, int>>();
            this.CurrencyPack = Substitute.For<ICurrencyPack>();
            this.PurchaseOrdersPack = Substitute.For<IPurchaseOrdersPack>();
            this.PurchaseOrderRepository = Substitute.For<IRepository<PurchaseOrder, int>>();
            this.HtmlTemplateService = Substitute.For<IHtmlTemplateService<PurchaseOrder>>();
            this.PurchaseLedgerMaster = Substitute.For<ISingleRecordRepository<PurchaseLedgerMaster>>();
            this.NominalAccountRepository = Substitute.For<IRepository<NominalAccount, int>>();
            this.PartQueryRepository = Substitute.For<IQueryRepository<Part>>();
            this.PartSupplierRepository = Substitute.For<IRepository<PartSupplier, PartSupplierKey>>();
            this.Log = Substitute.For<ILog>();
            this.NoteTemplateService = Substitute.For<IHtmlTemplateService<PlCreditDebitNote>>();
            this.OrderReceivedView = Substitute.For<IQueryRepository<PlOrderReceivedViewEntry>>();
            this.CancelledOrderDetailRepository = Substitute.For<IRepository<CancelledOrderDetail, int>>();
            this.NoteRepository = Substitute.For<IRepository<PlCreditDebitNote, int>>();
            this.Sut = new PurchaseOrderService(
                this.MockAuthService,
                this.PurchaseLedgerPack,
                this.MockDatabaseService,
                this.PdfService,
                this.EmailService,
                this.EmployeeRepository,
                this.MiniOrderRepository,
                this.SupplierRepository,
                this.LinnDeliveryAddressRepository,
                this.PurchaseOrdersPack,
                this.CurrencyPack,
                this.SupplierKitService,
                this.PurchaseOrderRepository,
                this.HtmlTemplateService,
                this.PurchaseLedgerMaster,
                this.NominalAccountRepository,
                this.PartQueryRepository,
                this.PartSupplierRepository,
                this.NoteTemplateService,
                this.Log,
                this.NoteRepository,
                this.OrderReceivedView,
                this.CancelledOrderDetailRepository);
        }
    }
}
