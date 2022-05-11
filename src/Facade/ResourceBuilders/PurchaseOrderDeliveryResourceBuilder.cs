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
                           Cancelled = entity.Cancelled,
                           DateAdvised = entity.DateAdvised,
                           DateRequested = entity.DateRequested,
                           DeliverySeq = entity.DeliverySeq,
                           NetTotalCurrency = entity.NetTotalCurrency,
                           BaseNetTotal = entity.BaseNetTotal,
                           OrderDeliveryQty = entity.OrderDeliveryQty,
                           OrderLine = entity.OrderLine,
                           OrderNumber = entity.OrderNumber,
                           OurDeliveryQty = entity.OurDeliveryQty,
                           QtyNetReceived = entity.QtyNetReceived,
                           QuantityOutstanding = entity.QuantityOutstanding,
                           CallOffDate = entity.CallOffDate,
                           BaseOurUnitPrice = entity.BaseOurUnitPrice,
                           SupplierConfirmationComment = entity.SupplierConfirmationComment,
                           OurUnitPriceCurrency = entity.OurUnitPriceCurrency,
                           OrderUnitPriceCurrency = entity.OrderUnitPriceCurrency,
                           BaseOrderUnitPrice = entity.BaseOrderUnitPrice,
                           VatTotalCurrency = entity.VatTotalCurrency,
                           BaseVatTotal = entity.BaseVatTotal,
                           DeliveryTotalCurrency = entity.DeliveryTotalCurrency,
                           BaseDeliveryTotal = entity.BaseDeliveryTotal
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
