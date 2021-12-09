namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class SuppliersResourceBuilder : IBuilder<IEnumerable<Supplier>>
    {
        public IEnumerable<SupplierResource> Build(IEnumerable<Supplier> entityList, IEnumerable<string> claims)
        {
            return entityList.Select(
                e => new SupplierResource { Id = e.SupplierId, Name = e.Name });
        }

        public string GetLocation(IEnumerable<Supplier> p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<IEnumerable<Supplier>>.Build(IEnumerable<Supplier> entityList, IEnumerable<string> claims)
            => this.Build(entityList, claims);
    }
}
