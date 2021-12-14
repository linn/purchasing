namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class TariffsResourceBuilder : IBuilder<IEnumerable<Tariff>>
    {
        public IEnumerable<TariffResource> Build(IEnumerable<Tariff> entityList, IEnumerable<string> claims)
        {
            return entityList.Select(
                e => new TariffResource { Id = e.Id, Description = e.Description });
        }

        public string GetLocation(IEnumerable<Tariff> p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<IEnumerable<Tariff>>.Build(IEnumerable<Tariff> entityList, IEnumerable<string> claims)
            => this.Build(entityList, claims);
    }
}
