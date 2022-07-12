namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class PurchaseOrderDeliveryResourceBuilder : IBuilder<PurchaseOrderDelivery>
    {
        public PurchaseOrderDeliveryResource Build(PurchaseOrderDelivery entity, IEnumerable<string> claims)
        {
            return new PurchaseOrderDeliveryResource
                       {
                           OrderNumber = entity.OrderNumber,
                           OrderLine = entity.OrderLine,
                           OurDeliveryQty = entity.OurDeliveryQty,
                           DateRequested = entity.DateRequested?.ToShortDateString(),
                           DateAdvised = entity.DateAdvised?.ToShortDateString(),
                           RescheduleReason = entity.RescheduleReason,
                           SupplierConfirmationComment = entity.SupplierConfirmationComment,
                           AvailableAtSupplier = entity.AvailableAtSupplier,
                           PartNumber = entity.PurchaseOrderDetail.PartNumber,
                           Cancelled = entity.Cancelled,
                           DeliverySeq = entity.DeliverySeq,
                           NetTotalCurrency = entity.NetTotalCurrency,
                           BaseNetTotal = entity.BaseNetTotal,
                           OrderDeliveryQty = entity.OrderDeliveryQty,
                           QtyNetReceived = entity.QtyNetReceived,
                           QuantityOutstanding = entity.QuantityOutstanding,
                           CallOffDate = entity.CallOffDate?.ToString("o"),
                           BaseOurUnitPrice = entity.BaseOurUnitPrice,
                           OurUnitPriceCurrency = entity.OurUnitPriceCurrency,
                           OrderUnitPriceCurrency = entity.OrderUnitPriceCurrency,
                           BaseOrderUnitPrice = entity.BaseOrderUnitPrice,
                           VatTotalCurrency = entity.VatTotalCurrency,
                           BaseVatTotal = entity.BaseVatTotal,
                           DeliveryTotalCurrency = entity.DeliveryTotalCurrency,
                           BaseDeliveryTotal = entity.BaseDeliveryTotal,
                           CallOffRef = entity.CallOffRef,
                           FilCancelled = entity.FilCancelled,
                           QtyPassedForPayment = entity.QtyPassedForPayment,
                       };
        }

        public string GetLocation(PurchaseOrderDelivery p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<PurchaseOrderDelivery>.Build(PurchaseOrderDelivery entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }
    }
}
