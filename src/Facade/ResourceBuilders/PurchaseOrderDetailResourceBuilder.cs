namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class PurchaseOrderDetailResourceBuilder : IBuilder<PurchaseOrderDetail>
    {
        private readonly IBuilder<PurchaseOrderDelivery> deliveryResourceBuilder;

        private readonly IBuilder<PurchaseOrderPosting> postingResourceBuilder;

        private readonly IAuthorisationService authService;

        public PurchaseOrderDetailResourceBuilder(
            IBuilder<PurchaseOrderDelivery> deliveryResourceBuilder,
            IBuilder<PurchaseOrderPosting> postingResourceBuilder,
            IAuthorisationService authService)
        {
            this.deliveryResourceBuilder = deliveryResourceBuilder;
            this.postingResourceBuilder = postingResourceBuilder;
            this.authService = authService;
        }

        public PurchaseOrderDetailResource Build(PurchaseOrderDetail entity, IEnumerable<string> claims)
        {
            var claimsList = claims.ToList();

            return new PurchaseOrderDetailResource
                       {
                           Line = entity.Line,
                           PartNumber = entity.PartNumber,
                           PartDescription = entity.Part?.Description,
                           Part = entity.Part != null
                                      ? new PartResource
                                            {
                                                PartNumber = entity.Part.PartNumber,
                                                Description = entity.Part.Description,
                                                StockControlled = entity.Part.StockControlled
                                            }
                                      : null,
                           Cancelled = entity.Cancelled,
                           BaseNetTotal = entity.BaseNetTotal,
                           NetTotalCurrency = entity.NetTotalCurrency,
                           OrderNumber = entity.OrderNumber,
                           OurQty = entity.OurQty,
                           OrderQty = entity.OrderQty,
                           PurchaseDeliveries =
                               entity.PurchaseDeliveries?.Select(
                                   d => (PurchaseOrderDeliveryResource)this.deliveryResourceBuilder.Build(d, claimsList)),
                           RohsCompliant = entity.RohsCompliant,
                           SuppliersDesignation = entity.SuppliersDesignation,
                           StockPoolCode = entity.StockPoolCode,
                           OriginalOrderNumber = entity.OriginalOrderNumber,
                           OriginalOrderLine = entity.OriginalOrderLine,
                           OurUnitOfMeasure = entity.OurUnitOfMeasure,
                           OrderUnitOfMeasure = entity.OrderUnitOfMeasure,
                           OurUnitPriceCurrency = entity.OurUnitPriceCurrency,
                           OrderUnitPriceCurrency = entity.OrderUnitPriceCurrency,
                           BaseOurUnitPrice = entity.BaseOurUnitPrice,
                           BaseOrderUnitPrice = entity.BaseOrderUnitPrice,
                           VatTotalCurrency = entity.VatTotalCurrency,
                           BaseVatTotal = entity.BaseVatTotal,
                           DetailTotalCurrency = entity.DetailTotalCurrency,
                           BaseDetailTotal = entity.BaseDetailTotal,
                           DeliveryInstructions = entity.DeliveryInstructions,
                           FilCancelled = entity.FilCancelled,
                           ReasonFilCancelled = entity.FilCancelled == "Y"
                                                    ? entity.CancelledDetails
                                                        ?.LastOrDefault(a => a.FilCancelledById.HasValue)
                                                        ?.ReasonFilCancelled
                                                    : null,
                           FilCancelledBy = entity.FilCancelled == "Y"
                                                ? entity.CancelledDetails?.LastOrDefault(a => a.FilCancelledById.HasValue)?.FilCancelledById
                                                : null,
                           DateFilCancelled = entity.FilCancelled == "Y"
                                                  ? entity.CancelledDetails?.LastOrDefault(a => a.FilCancelledById.HasValue)?.DateFilCancelled?.ToString("dd-MMM-yyyy")
                                                  : null,
                           FilCancelledByName = entity.FilCancelled == "Y"
                                                    ? entity.CancelledDetails?.LastOrDefault(a => a.FilCancelledById.HasValue)?.FilCancelledBy?.FullName
                                                    : null,
                           DrawingReference = entity.DrawingReference,
                           DeliveryConfirmedBy =
                               entity.DeliveryConfirmedBy != null
                                   ? new EmployeeResource
                                         {
                                             Id = entity.DeliveryConfirmedBy.Id,
                                             FullName = entity.DeliveryConfirmedBy.FullName
                                         }
                                   : null,
                           InternalComments = entity.InternalComments,
                           OrderPosting = entity.OrderPosting != null
                                              ? (PurchaseOrderPostingResource)this.postingResourceBuilder.Build(
                                                  entity.OrderPosting,
                                                  claimsList)
                                              : null,
                           Links = this.BuildLinks(entity, claimsList).ToArray(),
                       };
        }

        public string GetLocation(PurchaseOrderDetail model)
        {
            throw new NotImplementedException();
        }

        object IBuilder<PurchaseOrderDetail>.Build(PurchaseOrderDetail entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }

        private IEnumerable<LinkResource> BuildLinks(PurchaseOrderDetail item, IEnumerable<string> claims)
        {
            if (this.authService.HasPermissionFor(AuthorisedAction.PurchaseLedgerAdmin, claims))
            {
                if (item.CanBeAutoBooked())
                {
                    yield return new LinkResource { Rel = "auto-book-in", Href = "/ledgers/purchase/auto-book-stock" };
                }

                if (item.CanSwitchOurQtyAndOurPrice())
                {
                    yield return new LinkResource
                                     {
                                         Rel = "switch-our-qty-price",
                                         Href = $"/purchasing/purchase-orders/{item.OrderNumber}/switch-our-qty-price"
                                     };
                }
            }
        }
    }
}
