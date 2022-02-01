namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class PurchaseOrdersResourceBuilder : IBuilder<IEnumerable<PurchaseOrder>>
    {
        public IEnumerable<PurchaseOrderResource> Build(IEnumerable<PurchaseOrder> entityList, IEnumerable<string> claims)
        {
            return entityList.Select(
                e => new PurchaseOrderResource {  OrderNumber = e.OrderNumber });
        }

        public string GetLocation(IEnumerable<PurchaseOrder> p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<IEnumerable<PurchaseOrder>>.Build(IEnumerable<PurchaseOrder> entityList, IEnumerable<string> claims) 
            => this.Build(entityList, claims);
    }
}
