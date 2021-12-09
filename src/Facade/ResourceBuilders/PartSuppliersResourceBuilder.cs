namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class PartSuppliersResourceBuilder : IBuilder<IEnumerable<PartSupplier>>
    {
        public IEnumerable<PartSupplierResource> Build(IEnumerable<PartSupplier> entityList, IEnumerable<string> claims)
        {
            return entityList.Select(
                e => new PartSupplierResource { PartNumber = e.PartNumber, SupplierId = e.SupplierId });
        }

        public string GetLocation(IEnumerable<PartSupplier> p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<IEnumerable<PartSupplier>>.Build(IEnumerable<PartSupplier> entityList, IEnumerable<string> claims) 
            => this.Build(entityList, claims);
    }
}
