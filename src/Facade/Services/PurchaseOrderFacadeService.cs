﻿namespace Linn.Purchasing.Facade.Services
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
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class
        PurchaseOrderFacadeService : FacadeResourceService<PurchaseOrder, int, PurchaseOrderResource,
            PurchaseOrderResource>
    {
        private readonly IPurchaseOrderService domainService;

        private readonly IRepository<OverbookAllowedByLog, int> overbookAllowedByLogRepository;

        public PurchaseOrderFacadeService(
            IRepository<PurchaseOrder, int> repository,
            ITransactionManager transactionManager,
            IBuilder<PurchaseOrder> resourceBuilder,
            IPurchaseOrderService domainService,
            IRepository<OverbookAllowedByLog, int> overbookAllowedByLogRepository)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.domainService = domainService;
            this.overbookAllowedByLogRepository = overbookAllowedByLogRepository;
        }

        protected override PurchaseOrder CreateFromResource(
            PurchaseOrderResource resource,
            IEnumerable<string> privileges = null)
        {
            //var order = this.BuildEntityFromResourceHelper(resource);
            //this.domainService.CreateOrder(order, privileges);

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
                throw new NotImplementedException();

                //var updated = this.BuildEntityFromResourceHelper(updateResource);
                //this.domainService.Update(entity, updated, privileges);
            }
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
                                                                           OrderNumber = d.OrderNumber,
                                                                           OurDeliveryQty = d.OurDeliveryQty,
                                                                           QtyNetReceived = d.QtyNetReceived,
                                                                           QuantityOutstanding = d.QuantityOutstanding,
                                                                           CallOffDate = d.CallOffDate,
                                                                           BaseOurUnitPrice = d.BaseOurUnitPrice,
                                                                           SupplierConfirmationComment =
                                                                               d.SupplierConfirmationComment,
                                                                           OurUnitPriceCurrency =
                                                                               d.OurUnitPriceCurrency,
                                                                           OrderUnitPriceCurrency =
                                                                               d.OrderUnitPriceCurrency,
                                                                           BaseOrderUnitPrice = d.BaseOrderUnitPrice,
                                                                           VatTotalCurrency = d.VatTotalCurrency,
                                                                           BaseVatTotal = d.BaseVatTotal,
                                                                           DeliveryTotalCurrency =
                                                                               d.DeliveryTotalCurrency,
                                                                           BaseDeliveryTotal = d.BaseDeliveryTotal
                                                                       }) as IList<PurchaseOrderDelivery>,
                                                      RohsCompliant = x.RohsCompliant,
                                                      SuppliersDesignation = x.SuppliersDesignation,
                                                      StockPoolCode = x.StockPoolCode,
                                                      OriginalOrderNumber = x.OriginalOrderNumber,
                                                      OriginalOrderLine = x.OriginalOrderLine,
                                                      OurUnitOfMeasure = x.OurUnitOfMeasure,
                                                      OrderUnitOfMeasure = x.OrderUnitOfMeasure,
                                                      Duty = x.Duty,
                                                      OurUnitPriceCurrency = x.OurUnitPriceCurrency,
                                                      OrderUnitPriceCurrency = x.OrderUnitPriceCurrency,
                                                      BaseOurUnitPrice = x.BaseOurUnitPrice,
                                                      BaseOrderUnitPrice = x.BaseOrderUnitPrice,
                                                      VatTotalCurrency = x.VatTotalCurrency,
                                                      BaseVatTotal = x.BaseVatTotal,
                                                      DetailTotalCurrency = x.DetailTotalCurrency,
                                                      BaseDetailTotal = x.BaseDetailTotal,
                                                      DeliveryInstructions = x.DeliveryInstructions,
                                                      //// todo check employee bits once front end done, unsure whether will have doneBy object.Id or just the id int
                                                      DeliveryConfirmedById = x.DeliveryConfirmedBy.Id,
                                                      CancelledDetails = x.CancelledDetails.Select(
                                                                             c => new CancelledOrderDetail
                                                                                 {
                                                                                     OrderNumber = c.OrderNumber,
                                                                                     LineNumber = c.LineNumber,
                                                                                     DeliverySequence =
                                                                                         c.DeliverySequence,
                                                                                     DateCancelled =
                                                                                         c.DateCancelled,
                                                                                     CancelledById =
                                                                                         c.CancelledBy.Id,
                                                                                     DateFilCancelled =
                                                                                         c.DateFilCancelled,
                                                                                     FilCancelledById =
                                                                                         c.FilCancelledBy.Id,
                                                                                     ReasonCancelled =
                                                                                         c.ReasonCancelled,
                                                                                     Id = c.Id,
                                                                                     PeriodCancelled =
                                                                                         c.PeriodCancelled,
                                                                                     PeriodFilCancelled =
                                                                                         c.PeriodFilCancelled,
                                                                                     ValueCancelled =
                                                                                         c.ValueCancelled,
                                                                                     DateUncancelled =
                                                                                         c.DateUncancelled,
                                                                                     DateFilUncancelled =
                                                                                         c.DateFilUncancelled,
                                                                                     DatePreviouslyCancelled =
                                                                                         c.DatePreviouslyCancelled,
                                                                                     DatePreviouslyFilCancelled =
                                                                                         c.DatePreviouslyFilCancelled,
                                                                                     ValueFilCancelled =
                                                                                         c.ValueFilCancelled,
                                                                                     BaseValueFilCancelled =
                                                                                         c.BaseValueFilCancelled
                                                                                 }) as IList<CancelledOrderDetail>,
                                                      InternalComments = x.InternalComments
                                                      ///// no mr orders as don't want to save them from here
                                                  }) as IList<PurchaseOrderDetail>,
                           CurrencyCode = resource.Currency.Code,
                           OrderContactName = resource.OrderContactName,
                           OrderMethodName = resource.OrderMethodName,
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
                           DateFilCancelled = resource.DateFilCancelled,
                           PeriodFilCancelled = resource.PeriodFilCancelled
                       };
        }
    }
}
