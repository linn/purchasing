namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class TariffResourceBuilder : IBuilder<Tariff>
    {
        public TariffResource Build(Tariff entity, IEnumerable<string> claims)
        {
            return new TariffResource
                       {
                           Id = entity.Id,
                           Description = entity.Description,
                           Code = entity.Code
                       };
        }

        public string GetLocation(Tariff p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<Tariff>.Build(Tariff entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
