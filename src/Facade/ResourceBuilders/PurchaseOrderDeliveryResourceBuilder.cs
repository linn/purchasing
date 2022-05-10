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
                           OrderLine = entity.OrderLine
                       };
        }

        public string GetLocation(PurchaseOrderDelivery p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<PurchaseOrderDelivery>.Build(PurchaseOrderDelivery entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
