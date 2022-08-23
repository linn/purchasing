namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Facade;
    using Linn.Common.Logging;
    using Linn.Common.Pdf;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;
    using Linn.Purchasing.Resources.SearchResources;

    public class PurchaseOrderFacadeService :
        FacadeFilterResourceService<PurchaseOrder, int, PurchaseOrderResource, PurchaseOrderResource,
            PurchaseOrderSearchResource>,
        IPurchaseOrderFacadeService
    {
        private readonly IPurchaseOrderService domainService;

        private readonly IFileReader fileReader;

        private readonly ILog logger;

        private readonly IRepository<PurchaseOrder, int> orderRepository;

        private readonly IRepository<OverbookAllowedByLog, int> overbookAllowedByLogRepository;

        private readonly string pathToTemplate;

        private readonly IBuilder<PurchaseOrder> resourceBuilder;

        private readonly ITemplateEngine templateEngine;

        private readonly ITransactionManager transactionManager;

        public PurchaseOrderFacadeService(
            IRepository<PurchaseOrder, int> repository,
            ITransactionManager transactionManager,
            IBuilder<PurchaseOrder> resourceBuilder,
            IPurchaseOrderService domainService,
            IRepository<OverbookAllowedByLog, int> overbookAllowedByLogRepository,
            string pathToTemplate,
            IFileReader filerReader,
            ITemplateEngine templateEngine,
            ILog logger)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.domainService = domainService;
            this.overbookAllowedByLogRepository = overbookAllowedByLogRepository;
            this.transactionManager = transactionManager;
            this.orderRepository = repository;
            this.templateEngine = templateEngine;
            this.pathToTemplate = pathToTemplate;
            this.fileReader = filerReader;
            this.logger = logger;
            this.resourceBuilder = resourceBuilder;
        }

        public async Task<IResult<ProcessResultResource>> EmailOrderPdf(
            int orderNumber,
            string emailAddress,
            bool bcc,
            int currentUserId)
        {
            try
            {
                var order = this.orderRepository.FindById(orderNumber);

                var template = await this.fileReader.ReadFile(this.pathToTemplate);

                var result = await this.templateEngine.Render(order, template);

                var emailResult = this.domainService.SendPdfEmail(
                    result,
                    emailAddress,
                    orderNumber,
                    bcc,
                    currentUserId,
                    order);

                this.logger.Write(LoggingLevel.Info, new List<LoggingProperty>(), emailResult.Message);
                this.transactionManager.Commit();
                return new SuccessResult<ProcessResultResource>(
                    new ProcessResultResource(emailResult.Success, emailResult.Message));
            }
            catch (Exception ex)
            {
                this.logger.Write(LoggingLevel.Error, new List<LoggingProperty>(), ex.Message);
                return new BadRequestResult<ProcessResultResource>(ex.Message);
            }
        }

        public IResult<ProcessResultResource> EmailSupplierAss(int orderNumber)
        {
            try
            {
                var emailResult = this.domainService.SendSupplierAssemblyEmail(orderNumber);

                this.logger.Write(LoggingLevel.Info, new List<LoggingProperty>(), emailResult.Message);

                return new SuccessResult<ProcessResultResource>(
                    new ProcessResultResource(emailResult.Success, emailResult.Message));
            }
            catch (DomainException ex)
            {
                this.logger.Write(LoggingLevel.Error, new List<LoggingProperty>(), ex.Message);
                return new BadRequestResult<ProcessResultResource>(ex.Message);
            }
        }

        public IResult<PurchaseOrderResource> FillOutOrderFromSupplierId(
            PurchaseOrderResource resource,
            IEnumerable<string> privileges,
            int currentUserId)
        {
            var updated = new PurchaseOrder
                              {
                                  SupplierId = resource.Supplier.Id,
                                  Supplier = new Supplier
                                                 {
                                                     SupplierId = resource.Supplier.Id, Name = resource.Supplier.Name
                                                 },
                                  Details = resource.Details?.Select(
                                      x => new PurchaseOrderDetail
                                               {
                                                   Line = 1,
                                                   BaseNetTotal = x.BaseNetTotal,
                                                   NetTotalCurrency = x.NetTotalCurrency,
                                                   OurQty = x.OurQty,
                                                   OrderQty = x.OrderQty,
                                                   Part =
                                                       new Part
                                                           {
                                                               PartNumber = x.PartNumber,
                                                               Description = x.PartDescription
                                                           },
                                                   PartNumber = x.PartNumber,
                                                   OurUnitOfMeasure = x.OurUnitOfMeasure,
                                                   OrderUnitOfMeasure = x.OrderUnitOfMeasure,
                                                   OurUnitPriceCurrency = x.OurUnitPriceCurrency,
                                                   OrderUnitPriceCurrency = x.OrderUnitPriceCurrency,
                                                   BaseOurUnitPrice = x.BaseOurUnitPrice,
                                                   BaseOrderUnitPrice = x.BaseOrderUnitPrice,
                                                   VatTotalCurrency = x.VatTotalCurrency,
                                                   BaseVatTotal = x.BaseVatTotal,
                                                   DetailTotalCurrency = x.DetailTotalCurrency,
                                                   BaseDetailTotal = x.BaseDetailTotal
                                               }).ToList()
                              };

            var generatedOrder = this.domainService.FillOutUnsavedOrder(updated, currentUserId);

            return new SuccessResult<PurchaseOrderResource>(
                (PurchaseOrderResource)this.resourceBuilder.Build(generatedOrder, privileges));
        }

        public IResult<ProcessResultResource> AuthorisePurchaseOrders(
            PurchaseOrdersProcessRequestResource resource,
            IEnumerable<string> privileges,
            int userId)
        {
            var result = this.domainService.AuthoriseMultiplePurchaseOrders(resource.Orders.ToList(), userId);
            if (!result.Success)
            {
                return new BadRequestResult<ProcessResultResource>(result.Message);
            }

            this.transactionManager.Commit();
            return new SuccessResult<ProcessResultResource>(new ProcessResultResource(result.Success, result.Message));
        }

        public IResult<ProcessResultResource> EmailOrderPdfs(
            PurchaseOrdersProcessRequestResource resource,
            IEnumerable<string> privileges,
            int userId)
        {
            var result = this.domainService.EmailMultiplePurchaseOrders(
                resource.Orders.ToList(),
                userId,
                resource.CopySelf == "true");
            if (!result.Success)
            {
                return new BadRequestResult<ProcessResultResource>(result.Message);
            }

            this.transactionManager.Commit();
            return new SuccessResult<ProcessResultResource>(new ProcessResultResource(result.Success, result.Message));
        }

        public async Task<string> GetOrderAsHtml(int orderNumber)
        {
            var order = this.orderRepository.FindById(orderNumber);

            var template = await this.fileReader.ReadFile(this.pathToTemplate);

            var result = await this.templateEngine.Render(order, template);

            return result;
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

        protected override Expression<Func<PurchaseOrder, bool>> FilterExpression(
            PurchaseOrderSearchResource searchResource)
        {
            if (!string.IsNullOrEmpty(searchResource.StartDate))
            {
                return a => a.OrderDate >= DateTime.Parse(searchResource.StartDate) && a.OrderDate <= DateTime.Parse(searchResource.EndDate);
            }

            return x => x.OrderNumber.ToString().Contains(searchResource.OrderNumber);
        }

        protected override Expression<Func<PurchaseOrder, bool>> FindExpression(
            PurchaseOrderSearchResource searchResource)
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
                           SupplierId = resource.Supplier.Id,
                           Cancelled = resource.Cancelled,
                           DocumentTypeName = resource.DocumentType?.Name,
                           OrderDate = DateTime.Parse(resource.OrderDate),
                           Overbook = resource.Overbook,
                           OverbookQty = resource.OverbookQty,
                           Details =
                               resource.Details?.Select(
                                   x => new PurchaseOrderDetail
                                            {
                                                Cancelled = x.Cancelled,
                                                Line = x.Line,
                                                BaseNetTotal = x.BaseNetTotal,
                                                NetTotalCurrency = x.NetTotalCurrency,
                                                OrderNumber = x.OrderNumber,
                                                OurQty = x.OurQty,
                                                OrderQty = x.OrderQty,
                                                Part =
                                                    new Part
                                                        {
                                                            PartNumber = x.PartNumber, Description = x.PartDescription
                                                        },
                                                PartNumber = x.PartNumber,
                                                PurchaseDeliveries =
                                                    x.PurchaseDeliveries?.Select(
                                                        d => new PurchaseOrderDelivery
                                                                 {
                                                                     Cancelled = d.Cancelled,
                                                                     DateAdvised =
                                                                         DateTime.TryParse(
                                                                             d.DateAdvised,
                                                                             out var dateAdvised)
                                                                             ? dateAdvised
                                                                             : null,
                                                                     DateRequested =
                                                                         DateTime.TryParse(
                                                                             d.DateRequested,
                                                                             out var dateRequested)
                                                                             ? dateRequested
                                                                             : null,
                                                                     DeliverySeq = d.DeliverySeq,
                                                                     NetTotalCurrency = d.NetTotalCurrency,
                                                                     BaseNetTotal = d.BaseNetTotal,
                                                                     OrderDeliveryQty = d.OrderDeliveryQty,
                                                                     OrderLine = d.OrderLine,
                                                                     OrderNumber = d.OrderNumber,
                                                                     OurDeliveryQty = d.OurDeliveryQty,
                                                                     QtyNetReceived = d.QtyNetReceived,
                                                                     QuantityOutstanding = d.QuantityOutstanding,
                                                                     CallOffDate =
                                                                         DateTime.TryParse(
                                                                             d.CallOffDate,
                                                                             out var callOffDate)
                                                                             ? callOffDate
                                                                             : null,
                                                                     BaseOurUnitPrice = d.BaseOurUnitPrice,
                                                                     SupplierConfirmationComment =
                                                                         d.SupplierConfirmationComment,
                                                                     OurUnitPriceCurrency = d.OurUnitPriceCurrency,
                                                                     OrderUnitPriceCurrency = d.OrderUnitPriceCurrency,
                                                                     BaseOrderUnitPrice = d.BaseOrderUnitPrice,
                                                                     VatTotalCurrency = d.VatTotalCurrency,
                                                                     BaseVatTotal = d.BaseVatTotal,
                                                                     DeliveryTotalCurrency = d.DeliveryTotalCurrency,
                                                                     BaseDeliveryTotal = d.BaseDeliveryTotal,
                                                                     RescheduleReason = d.RescheduleReason,
                                                                     AvailableAtSupplier = d.AvailableAtSupplier
                                                                 }).ToList(),
                                                RohsCompliant = x.RohsCompliant,
                                                SuppliersDesignation = x.SuppliersDesignation,
                                                StockPoolCode = x.StockPoolCode,
                                                OriginalOrderNumber = x.OriginalOrderNumber,
                                                OriginalOrderLine = x.OriginalOrderLine,
                                                OurUnitOfMeasure = x.OurUnitOfMeasure,
                                                OrderUnitOfMeasure = x.OrderUnitOfMeasure,
                                                OurUnitPriceCurrency = x.OurUnitPriceCurrency,
                                                OrderUnitPriceCurrency = x.OrderUnitPriceCurrency,
                                                BaseOurUnitPrice = x.BaseOurUnitPrice,
                                                BaseOrderUnitPrice = x.BaseOrderUnitPrice,
                                                VatTotalCurrency = x.VatTotalCurrency,
                                                BaseVatTotal = x.BaseVatTotal,
                                                DetailTotalCurrency = x.DetailTotalCurrency,
                                                BaseDetailTotal = x.BaseDetailTotal,
                                                DeliveryInstructions = x.DeliveryInstructions,
                                                DeliveryConfirmedById = x.DeliveryConfirmedBy?.Id,
                                                CancelledDetails =
                                                    x.CancelledDetails?.Select(
                                                        c => new CancelledOrderDetail
                                                                 {
                                                                     OrderNumber = c.OrderNumber,
                                                                     LineNumber = c.LineNumber,
                                                                     DeliverySequence = c.DeliverySequence,
                                                                     DateCancelled = c.DateCancelled,
                                                                     CancelledById = c.CancelledBy.Id,
                                                                     DateFilCancelled = c.DateFilCancelled,
                                                                     FilCancelledById = c.FilCancelledBy.Id,
                                                                     ReasonCancelled = c.ReasonCancelled,
                                                                     Id = c.Id,
                                                                     PeriodCancelled = c.PeriodCancelled,
                                                                     PeriodFilCancelled = c.PeriodFilCancelled,
                                                                     ValueCancelled = c.ValueCancelled,
                                                                     DateUncancelled = c.DateUncancelled,
                                                                     DateFilUncancelled = c.DateFilUncancelled,
                                                                     DatePreviouslyCancelled =
                                                                         c.DatePreviouslyCancelled,
                                                                     DatePreviouslyFilCancelled =
                                                                         c.DatePreviouslyFilCancelled,
                                                                     ValueFilCancelled = c.ValueFilCancelled,
                                                                     BaseValueFilCancelled = c.BaseValueFilCancelled
                                                                 }).ToList(),
                                                InternalComments = x.InternalComments,
                                                OrderPosting = new PurchaseOrderPosting
                                                                   {
                                                                       Building = x.OrderPosting.Building,
                                                                       Id = x.OrderPosting.Id,
                                                                       LineNumber = x.OrderPosting.LineNumber,
                                                                       NominalAccount =
                                                                           new NominalAccount
                                                                               {
                                                                                   AccountId =
                                                                                       x.OrderPosting.NominalAccountId,
                                                                                   DepartmentCode =
                                                                                       x.OrderPosting.NominalAccount
                                                                                           .Department.DepartmentCode,
                                                                                   NominalCode =
                                                                                       x.OrderPosting.NominalAccount
                                                                                           .Nominal.NominalCode,
                                                                                   Nominal = new Nominal
                                                                                       {
                                                                                           Description =
                                                                                               x.OrderPosting
                                                                                                   .NominalAccount
                                                                                                   .Nominal
                                                                                                   .Description,
                                                                                           NominalCode =
                                                                                               x.OrderPosting
                                                                                                   .NominalAccount
                                                                                                   .Nominal
                                                                                                   .NominalCode
                                                                                       },
                                                                                   Department = new Department
                                                                                       {
                                                                                           Description =
                                                                                               x.OrderPosting
                                                                                                   .NominalAccount
                                                                                                   .Department
                                                                                                   .Description,
                                                                                           DepartmentCode =
                                                                                               x.OrderPosting
                                                                                                   .NominalAccount
                                                                                                   .Department
                                                                                                   .DepartmentCode
                                                                                       }
                                                                               },
                                                                       NominalAccountId =
                                                                           x.OrderPosting.NominalAccountId,
                                                                       Notes = x.OrderPosting.Notes,
                                                                       OrderNumber = x.OrderNumber,
                                                                       Person = x.OrderPosting.Person,
                                                                       Product = x.OrderPosting.Product,
                                                                       Qty = x.OrderPosting.Qty,
                                                                       Vehicle = x.OrderPosting.Vehicle
                                                                   }
                                            }).ToList(),
                           CurrencyCode = resource.Currency.Code,
                           OrderContactName = resource.OrderContactName,
                           OrderMethodName = resource.OrderMethod.Name,
                           ExchangeRate = resource.ExchangeRate,
                           IssuePartsToSupplier = resource.IssuePartsToSupplier,
                           DeliveryAddressId = resource.DeliveryAddress.AddressId,
                           RequestedById = resource.RequestedBy.Id,
                           EnteredById = resource.EnteredBy.Id,
                           QuotationRef = resource.QuotationRef,
                           AuthorisedById = resource.AuthorisedBy?.Id,
                           SentByMethod = resource.SentByMethod,
                           FilCancelled = resource.FilCancelled,
                           Remarks = resource.Remarks,
                           DateFilCancelled =
                               DateTime.TryParse(resource.DateFilCancelled, out var dateFilCancelled)
                                   ? dateFilCancelled
                                   : null,
                           PeriodFilCancelled = resource.PeriodFilCancelled
                       };
        }
    }
}
