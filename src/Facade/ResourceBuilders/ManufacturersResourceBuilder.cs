namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class ManufacturersResourceBuilder : IBuilder<IEnumerable<Manufacturer>>
    {
        public IEnumerable<ManufacturerResource> Build(IEnumerable<Manufacturer> entityList, IEnumerable<string> claims)
        {
            return entityList.Select(
                e => new ManufacturerResource { Code = e.Code, Name = e.Name });
        }

        public string GetLocation(IEnumerable<Manufacturer> p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<IEnumerable<Manufacturer>>.Build(IEnumerable<Manufacturer> entityList, IEnumerable<string> claims)
            => this.Build(entityList, claims);
    }
}
