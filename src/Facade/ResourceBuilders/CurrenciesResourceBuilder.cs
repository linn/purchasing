namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;


    public class CurrenciesResourceBuilder : IBuilder<IEnumerable<Currency>>
    {
        public IEnumerable<CurrencyResource> Build(IEnumerable<Currency> entityList, IEnumerable<string> claims)
        {
            return entityList.Select(
                e => new CurrencyResource { Code = e.Code, Name = e.Name });
        }

        public string GetLocation(IEnumerable<Currency> p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<IEnumerable<Currency>>.Build(IEnumerable<Currency> entityList, IEnumerable<string> claims)
            => this.Build(entityList, claims);
    }
}
