namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    public class MrPurchaseOrderResourceBuilder : IBuilder<MrPurchaseOrderDetail>
    {
        public MrPurchaseOrderResource Build(MrPurchaseOrderDetail entity, IEnumerable<string> claims)
        {
            return new MrPurchaseOrderResource
                       {
                           JobRef = entity.JobRef,
                           OrderNumber = entity.OrderNumber,
                           OrderLine = entity.OrderLine,
                           DateOfOrder = entity.DateOfOrder.ToString("o"),
                           SupplierId = entity.SupplierId,
                           SupplierName = entity.SupplierName,
                           Quantity = entity.OurQuantity,
                           QuantityReceived = entity.QuantityReceived,
                           QuantityInvoiced = entity.QuantityInvoiced,
                           Remarks = entity.Remarks,
                           SupplierContact = entity.SupplierContact,
                           UnauthorisedWarning = string.IsNullOrEmpty(entity.AuthorisedBy) ? "**Unauthorised**" : null,
                           Links = this.BuildLinks(entity).ToArray(),
                           Deliveries = entity.Deliveries.Select(d => new MrDeliveryResource
                                                                          {
                                                                              JobRef = d.JobRef,
                                                                              OrderNumber = d.OrderNumber,
                                                                              OrderLine = d.OrderLine,
                                                                              DeliverySequence = d.DeliverySequence,
                                                                              DeliveryQuantity = d.Quantity,
                                                                              QuantityReceived = d.QuantityReceived,
                                                                              AdvisedDeliveryDate = d.AdvisedDeliveryDate?.ToString("o"),
                                                                              RequestedDeliveryDate = d.RequestedDeliveryDate?.ToString("o"),
                                                                              Reference = d.Reference
                                                                          })
                       };
        }

        public string GetLocation(MrPurchaseOrderDetail model)
        {
            throw new NotImplementedException();
        }

        object IBuilder<MrPurchaseOrderDetail>.Build(MrPurchaseOrderDetail entity, IEnumerable<string> claims) => this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(MrPurchaseOrderDetail model)
        {
            yield return new LinkResource
                             {
                                 Rel = "view-order",
                                 Href = $"/purchasing/purchase-orders/{model.OrderNumber}"
                             };
            yield return new LinkResource
                             {
                                 Rel = "acknowledge-deliveries",
                                 Href = $"/purchasing/purchase-orders/acknowledge?orderNumber={model.OrderNumber}"
                             };
        }
    }
}
