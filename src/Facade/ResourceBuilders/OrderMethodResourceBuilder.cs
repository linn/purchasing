namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class OrderMethodResourceBuilder : IBuilder<OrderMethod>
    {
        public OrderMethodResource Build(OrderMethod entity, IEnumerable<string> claims)
        {
            return new OrderMethodResource
                       {
                           Description = entity.Description,
                           Name = entity.Name
                       };
        }

        public string GetLocation(OrderMethod p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<OrderMethod>.Build(OrderMethod entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
