namespace Linn.Purchasing.Proxy
{
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    public class CurrencyPack : ICurrencyPack
    {
        public decimal CalculateBaseValueFromCurrencyValue(string newCurrency, decimal newPrice)
        {
            throw new System.NotImplementedException();
        }
    }
}
