namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;


    public class OrderMethodsResourceBuilder : IBuilder<IEnumerable<OrderMethod>>
    {
        public IEnumerable<OrderMethodResource> Build(IEnumerable<OrderMethod> entityList, IEnumerable<string> claims)
        {
            return entityList.Select(
                e => new OrderMethodResource { Description = e.Description, Name = e.Name });
        }

        public string GetLocation(IEnumerable<OrderMethod> p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<IEnumerable<OrderMethod>>.Build(IEnumerable<OrderMethod> entityList, IEnumerable<string> claims)
            => this.Build(entityList, claims);
    }
}
