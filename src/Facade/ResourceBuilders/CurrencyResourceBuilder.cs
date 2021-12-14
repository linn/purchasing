namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class CurrencyResourceBuilder : IBuilder<Currency>
    {
        public CurrencyResource Build(Currency entity, IEnumerable<string> claims)
        {
            return new CurrencyResource
                       {
                           Code = entity.Code,
                           Name = entity.Name
                       };
        }

        public string GetLocation(Currency p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<Currency>.Build(Currency entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
