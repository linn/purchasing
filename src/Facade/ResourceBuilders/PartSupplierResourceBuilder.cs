namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class PartSupplierResourceBuilder : IBuilder<PartSupplier>
    {
        public PartSupplierResource Build(PartSupplier entity, IEnumerable<string> claims)
        {
            return new PartSupplierResource
                       {
                           PartNumber = entity.PartNumber,
                           SupplierId = entity.SupplierId
                       };
        }

        public string GetLocation(PartSupplier p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<PartSupplier>.Build(PartSupplier entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
