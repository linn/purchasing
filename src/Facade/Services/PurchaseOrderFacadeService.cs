namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class
        PurchaseOrderFacadeService : FacadeResourceService<PurchaseOrder, int, PurchaseOrderResource,
            PurchaseOrderResource>
    {
        private readonly IPurchaseOrderService domainService;

        private readonly IAuthorisationService authService;

        private readonly IRepository<OverbookAllowedByLog, int> overbookAllowedByLogRepository;

        public PurchaseOrderFacadeService(
            IRepository<PurchaseOrder, int> repository,
            ITransactionManager transactionManager,
            IBuilder<PurchaseOrder> resourceBuilder,
            IPurchaseOrderService domainService,
            IRepository<OverbookAllowedByLog, int> overbookAllowedByLogRepository,
            IAuthorisationService authService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.domainService = domainService;
            this.overbookAllowedByLogRepository = overbookAllowedByLogRepository;
            this.authService = authService;
        }

        protected override PurchaseOrder CreateFromResource(
            PurchaseOrderResource resource,
            IEnumerable<string> privileges = null)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderCreate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to create purchase orders");
            }

            // create order
            var order = this.BuildEntityFromResourceHelper(updateResource);

            // will this add the details etc?

            // create detail(s)

            // create deliveries maybe only if split deliveries is 0? Are split delivieres pl deliveries or something else?

            // create pl_order_postings
            // select plorp_seq.nextval into v_plorp from dual;
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(PurchaseOrder entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            PurchaseOrder entity,
            PurchaseOrderResource resource,
            PurchaseOrderResource updateResource)
        {
            // if is overbook form
            var log = new OverbookAllowedByLog
                          {
                              OrderNumber = entity.OrderNumber,
                              OverbookQty = entity.OverbookQty,
                              OverbookDate = DateTime.Now,
                              OverbookGrantedBy = userNumber
                          };
            this.overbookAllowedByLogRepository.Add(log);
        }

        protected override Expression<Func<PurchaseOrder, bool>> SearchExpression(string searchTerm)
        {
            return x => x.OrderNumber.ToString().Contains(searchTerm);
        }

        protected override void UpdateFromResource(
            PurchaseOrder entity,
            PurchaseOrderResource updateResource,
            IEnumerable<string> privileges = null)
        {
            var updated = this.BuildEntityFromResourceHelper(updateResource);

            //check if overbook form or not, direct to different domain methods
            this.domainService.AllowOverbook(entity, updated, privileges);
        }

        private PurchaseOrder BuildEntityFromResourceHelper(PurchaseOrderResource resource)
        {
            return new PurchaseOrder
                       {
                           OrderNumber = resource.OrderNumber,
                           Cancelled = resource.Cancelled,
                           DocumentTypeName = resource.DocumentType,
                           OrderDate = resource.DateOfOrder,
                           Overbook = resource.Overbook,
                           OverbookQty = resource.OverbookQty,
                           SupplierId = resource.SupplierId,
                           Details = resource.Details?.Select(
                                         x => new PurchaseOrderDetail
                                                  {
                                                      Cancelled = x.Cancelled,
                                                      Line = x.Line,
                                                      BaseNetTotal = x.BaseNetTotal,
                                                      NetTotalCurrency = x.NetTotalCurrency,
                                                      OrderNumber = x.OrderNumber,
                                                      OurQty = x.OurQty,
                                                      Part =
                                                          new Part
                                                              {
                                                                  PartNumber = x.PartNumber,
                                                                  Description = x.PartDescription
                                                              },
                                                      PartNumber = x.PartNumber,
                                                      PurchaseDeliveries =
                                                          x.PurchaseDeliveries?.Select(
                                                              d => new PurchaseOrderDelivery
                                                                       {
                                                                           Cancelled = d.Cancelled,
                                                                           DateAdvised = d.DateAdvised,
                                                                           DateRequested = d.DateRequested,
                                                                           DeliverySeq = d.DeliverySeq,
                                                                           NetTotalCurrency = d.NetTotalCurrency,
                                                                           BaseNetTotal = d.BaseNetTotal,
                                                                           OrderDeliveryQty = d.OrderDeliveryQty,
                                                                           OrderLine = d.OrderLine,
                                                                           OrderNumber = 0,
                                                                           OurDeliveryQty = null,
                                                                           PurchaseOrderDetail = null,
                                                                           QtyNetReceived = null,
                                                                           QuantityOutstanding = null,
                                                                           CallOffDate = null,
                                                                           BaseOurUnitPrice = null,
                                                                           SupplierConfirmationComment = null,
                                                                           OurUnitPriceCurrency = null,
                                                                           OrderUnitPriceCurrency = null,
                                                                           BaseOrderUnitPrice = null,
                                                                           VatTotalCurrency = null,
                                                                           BaseVatTotal = null,
                                                                           DeliveryTotalCurrency = null,
                                                                           BaseDeliveryTotal = null
                                                                       }) as IList<PurchaseOrderDelivery>,
                                                      RohsCompliant = null,
                                                      SuppliersDesignation = null,
                                                      StockPoolCode = null,
                                                      PurchaseOrder = null,
                                                      OriginalOrderNumber = null,
                                                      OriginalOrderLine = null,
                                                      OurUnitOfMeasure = null,
                                                      OrderUnitOfMeasure = null,
                                                      Duty = null,
                                                      OurUnitPriceCurrency = null,
                                                      OrderUnitPriceCurrency = null,
                                                      BaseOurUnitPrice = null,
                                                      BaseOrderUnitPrice = null,
                                                      VatTotalCurrency = null,
                                                      BaseVatTotal = null,
                                                      DetailTotalCurrency = null,
                                                      BaseDetailTotal = null,
                                                      DeliveryInstructions = null,
                                                      DeliveryConfirmedBy = null,
                                                      DeliveryConfirmedById = 0,
                                                      CancelledDetails = null,
                                                      InternalComments = null,
                                                      MrOrders = null,
                                                  }) as IList<PurchaseOrderDetail>
                       };
        }

        public string DocumentTypeName { get; set; }

        public DocumentType DocumentType { get; set; }

        public DateTime OrderDate { get; set; }

        public int OrderNumber { get; set; }

        public Supplier Supplier { get; set; }

        public int SupplierId { get; set; }

        public string Overbook { get; set; }

        public decimal? OverbookQty { get; set; }

        public Currency Currency { get; set; }

        public string CurrencyCode { get; set; }

        //todo make sure this is added in domain from supplier contact when creating
        //or could show as field on front end and pass back
        public string OrderContactName { get; set; }

        public string OrderMethodName { get; set; }

        public OrderMethod OrderMethod { get; set; }

        public decimal? ExchangeRate { get; set; }

        public string IssuePartsToSupplier { get; set; }

        public int DeliveryAddressId { get; set; }

        public LinnDeliveryAddress DeliveryAddress { get; set; }

        public Employee RequestedBy { get; set; }

        public int RequestedById { get; set; }

        public Employee EnteredBy { get; set; }

        public int EnteredById { get; set; }

        public string QuotationRef { get; set; }

        public Employee AuthorisedBy { get; set; }

        public int? AuthorisedById { get; set; }

        public string SentByMethod { get; set; }

        public string FilCancelled { get; set; }

        public string Remarks { get; set; }

        public DateTime? DateFilCancelled { get; set; }

        public int? PeriodFilCancelled { get; set; }
    };
        }
    }
}
