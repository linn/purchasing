namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    public class CountryResourceBuilder : IBuilder<Country>
    {
        public CountryResource Build(Country entity, IEnumerable<string> claims)
        {
            return new CountryResource
                       {
                           CountryCode = entity.CountryCode,
                           Name = entity.Name
                       };
        }

        public string GetLocation(Country p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<Country>.Build(Country entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
