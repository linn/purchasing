namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class CurrencyResourceBuilder : IBuilder<Currency>
    {
        private readonly ICurrencyPack currencyPack;

        public CurrencyResourceBuilder(ICurrencyPack currencyPack)
        {
            this.currencyPack = currencyPack;
        }

        public CurrencyResource Build(Currency entity, IEnumerable<string> claims)
        {
            var exchangeRate = this.currencyPack.GetExchangeRate(entity.Code, "GBP");

            return new CurrencyResource
                       {
                           Code = entity.Code,
                           Name = entity.Name,
                           ExchangeRate = exchangeRate
                       };
        }

        public string GetLocation(Currency p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<Currency>.Build(Currency entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
