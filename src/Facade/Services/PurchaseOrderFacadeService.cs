namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Configuration;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class
        PurchaseOrderFacadeService : FacadeResourceService<PurchaseOrder, int, PurchaseOrderResource, PurchaseOrderResource>, IPurchaseOrderFacadeService
    {
        private readonly IPurchaseOrderService domainService;

        private readonly IRepository<OverbookAllowedByLog, int> overbookAllowedByLogRepository;

        private readonly ITransactionManager transactionManager;

        private readonly IRepository<PurchaseOrder, int> orderRepository;

        private readonly IRazorTemplateService razorTemplateEngine;

        public PurchaseOrderFacadeService(
            IRepository<PurchaseOrder, int> repository,
            ITransactionManager transactionManager,
            IBuilder<PurchaseOrder> resourceBuilder,
            IPurchaseOrderService domainService,
            IRepository<OverbookAllowedByLog, int> overbookAllowedByLogRepository,
            IRazorTemplateService razorTemplateEngine)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.domainService = domainService;
            this.overbookAllowedByLogRepository = overbookAllowedByLogRepository;
            this.transactionManager = transactionManager;
            this.orderRepository = repository;
            this.razorTemplateEngine = razorTemplateEngine;
        }

        public string GetOrderAsHtml(int orderNumber)
        {
            var order = this.orderRepository.FindById(orderNumber);

            var result = this.razorTemplateEngine.Render(order, $"{ConfigurationManager.Configuration["VIEWS_ROOT"]}PurchaseOrder.cshtml");
            return result.Result;
        }

        public IResult<ProcessResultResource> EmailOrderPdf(int orderNumber, string emailAddress, bool bcc, int currentUserId)
        {
            var order = this.orderRepository.FindById(orderNumber);

            var result = this.razorTemplateEngine.Render(order, $"{ConfigurationManager.Configuration["VIEWS_ROOT"]}PurchaseOrder.cshtml");

            var emailResult = this.domainService.SendPdfEmail(result.Result, emailAddress, orderNumber, bcc, currentUserId, order);

            this.transactionManager.Commit();
            return new SuccessResult<ProcessResultResource>(new ProcessResultResource(emailResult.Success, emailResult.Message));
        }

        protected override PurchaseOrder CreateFromResource(
            PurchaseOrderResource resource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();

            // var order = this.BuildEntityFromResourceHelper(resource);
            // this.domainService.CreateOrder(order, privileges);
        }

        protected override void DeleteOrObsoleteResource(PurchaseOrder entity, IEnumerable<string> privileges = null)
        {
            this.transactionManager.Commit();
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            PurchaseOrder entity,
            PurchaseOrderResource resource,
            PurchaseOrderResource updateResource)
        {
            if (updateResource.CurrentlyUsingOverbookForm)
            {
                var log = new OverbookAllowedByLog
                              {
                                  OrderNumber = entity.OrderNumber,
                                  OverbookQty = entity.OverbookQty,
                                  OverbookDate = DateTime.Now,
                                  OverbookGrantedBy = userNumber
                              };
                this.overbookAllowedByLogRepository.Add(log);
            }
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
            if (updateResource.CurrentlyUsingOverbookForm)
            {
                this.domainService.AllowOverbook(
                    entity,
                    updateResource.Overbook,
                    updateResource.OverbookQty,
                    privileges);
            }
            else
            {
                var updated = this.BuildEntityFromResourceHelper(updateResource);
                this.domainService.UpdateOrder(entity, updated, privileges);
            }
        }

        private PurchaseOrder BuildEntityFromResourceHelper(PurchaseOrderResource resource)
        {
            return new PurchaseOrder
                       {
                           OrderNumber = resource.OrderNumber,
                           //Cancelled = resource.Cancelled,
                           //DocumentTypeName = resource.DocumentType.Name,
                           //OrderDate = resource.OrderDate,
                           //Overbook = resource.Overbook,
                           //OverbookQty = resource.OverbookQty,
                           SupplierId = resource.Supplier.Id,
                           //Details =
                           //    resource.Details?.Select(
                           //        x => new PurchaseOrderDetail
                           //                 {
                           //                     Cancelled = x.Cancelled,
                           //                     Line = x.Line,
                           //                     BaseNetTotal = x.BaseNetTotal,
                           //                     NetTotalCurrency = x.NetTotalCurrency,
                           //                     OrderNumber = x.OrderNumber,
                           //                     OurQty = x.OurQty,
                           //                     Part =
                           //                         new Part
                           //                             {
                           //                                 PartNumber = x.PartNumber, Description = x.PartDescription
                           //                             },
                           //                     PartNumber = x.PartNumber,
                           //                     PurchaseDeliveries =
                           //                         x.PurchaseDeliveries?.Select(
                           //                             d => new PurchaseOrderDelivery
                           //                                      {
                           //                                          Cancelled = d.Cancelled,
                           //                                          DateAdvised = string.IsNullOrEmpty(d.DateAdvised)
                           //                                              ? null : DateTime.Parse(d.DateAdvised),
                           //                                          DateRequested = string.IsNullOrEmpty(d.DateRequested) ?
                           //                                              null : DateTime.Parse(d.DateRequested),
                           //                                          DeliverySeq = d.DeliverySeq,
                           //                                          NetTotalCurrency = d.NetTotalCurrency,
                           //                                          BaseNetTotal = d.BaseNetTotal,
                           //                                          OrderDeliveryQty = d.OrderDeliveryQty,
                           //                                          OrderLine = d.OrderLine,
                           //                                          OrderNumber = d.OrderNumber,
                           //                                          OurDeliveryQty = d.OurDeliveryQty,
                           //                                          QtyNetReceived = d.QtyNetReceived,
                           //                                          QuantityOutstanding = d.QuantityOutstanding,
                           //                                          CallOffDate = string.IsNullOrEmpty(d.CallOffDate)
                           //                                              ? null : DateTime.Parse(d.CallOffDate),
                           //                                         BaseOurUnitPrice = d.BaseOurUnitPrice,
                           //                                          SupplierConfirmationComment =
                           //                                              d.SupplierConfirmationComment,
                           //                                          OurUnitPriceCurrency = d.OurUnitPriceCurrency,
                           //                                          OrderUnitPriceCurrency = d.OrderUnitPriceCurrency,
                           //                                          BaseOrderUnitPrice = d.BaseOrderUnitPrice,
                           //                                          VatTotalCurrency = d.VatTotalCurrency,
                           //                                          BaseVatTotal = d.BaseVatTotal,
                           //                                          DeliveryTotalCurrency = d.DeliveryTotalCurrency,
                           //                                          BaseDeliveryTotal = d.BaseDeliveryTotal,
                           //                                          RescheduleReason = d.RescheduleReason,
                           //                                          AvailableAtSupplier = d.AvailableAtSupplier
                           //                                      }) as ICollection<PurchaseOrderDelivery>,
                           //                     RohsCompliant = x.RohsCompliant,
                           //                     SuppliersDesignation = x.SuppliersDesignation,
                           //                     StockPoolCode = x.StockPoolCode,
                           //                     OriginalOrderNumber = x.OriginalOrderNumber,
                           //                     OriginalOrderLine = x.OriginalOrderLine,
                           //                     OurUnitOfMeasure = x.OurUnitOfMeasure,
                           //                     OrderUnitOfMeasure = x.OrderUnitOfMeasure,
                           //                     Duty = x.Duty,
                           //                     OurUnitPriceCurrency = x.OurUnitPriceCurrency,
                           //                     OrderUnitPriceCurrency = x.OrderUnitPriceCurrency,
                           //                     BaseOurUnitPrice = x.BaseOurUnitPrice,
                           //                     BaseOrderUnitPrice = x.BaseOrderUnitPrice,
                           //                     VatTotalCurrency = x.VatTotalCurrency,
                           //                     BaseVatTotal = x.BaseVatTotal,
                           //                     DetailTotalCurrency = x.DetailTotalCurrency,
                           //                     BaseDetailTotal = x.BaseDetailTotal,
                           //                     DeliveryInstructions = x.DeliveryInstructions,
                           //                     DeliveryConfirmedById = x.DeliveryConfirmedBy.Id,
                           //                     CancelledDetails = x.CancelledDetails.Select(
                           //                                            c => new CancelledOrderDetail
                           //                                                     {
                           //                                                         OrderNumber = c.OrderNumber,
                           //                                                         LineNumber = c.LineNumber,
                           //                                                         DeliverySequence =
                           //                                                             c.DeliverySequence,
                           //                                                         DateCancelled = c.DateCancelled,
                           //                                                         CancelledById = c.CancelledBy.Id,
                           //                                                         DateFilCancelled =
                           //                                                             c.DateFilCancelled,
                           //                                                         FilCancelledById =
                           //                                                             c.FilCancelledBy.Id,
                           //                                                         ReasonCancelled = c.ReasonCancelled,
                           //                                                         Id = c.Id,
                           //                                                         PeriodCancelled = c.PeriodCancelled,
                           //                                                         PeriodFilCancelled =
                           //                                                             c.PeriodFilCancelled,
                           //                                                         ValueCancelled = c.ValueCancelled,
                           //                                                         DateUncancelled = c.DateUncancelled,
                           //                                                         DateFilUncancelled =
                           //                                                             c.DateFilUncancelled,
                           //                                                         DatePreviouslyCancelled =
                           //                                                             c.DatePreviouslyCancelled,
                           //                                                         DatePreviouslyFilCancelled =
                           //                                                             c.DatePreviouslyFilCancelled,
                           //                                                         ValueFilCancelled =
                           //                                                             c.ValueFilCancelled,
                           //                                                         BaseValueFilCancelled =
                           //                                                             c.BaseValueFilCancelled
                           //                                                     }) as ICollection<CancelledOrderDetail>,
                           //                     InternalComments = x.InternalComments
                           //                 }) as ICollection<PurchaseOrderDetail>,
                           //CurrencyCode = resource.Currency.Code,
                           //OrderContactName = resource.OrderContactName,
                           //OrderMethodName = resource.OrderMethod.Name,
                           //ExchangeRate = resource.ExchangeRate,
                           //IssuePartsToSupplier = resource.IssuePartsToSupplier,
                           //DeliveryAddressId = resource.DeliveryAddress.AddressId,
                           //RequestedById = resource.RequestedBy.Id,
                           //EnteredById = resource.EnteredBy.Id,
                           //QuotationRef = resource.QuotationRef,
                           //AuthorisedById = resource.AuthorisedBy?.Id,
                           //SentByMethod = resource.SentByMethod,
                           //FilCancelled = resource.FilCancelled,
                           //Remarks = resource.Remarks,
                           //DateFilCancelled = resource.DateFilCancelled,
                           //PeriodFilCancelled = resource.PeriodFilCancelled
                       };
        }
    }
}
