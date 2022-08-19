namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface ICurrencyPack
    {
        decimal CalculateBaseValueFromCurrencyValue(
            string newCurrency,
            decimal newPrice,
            string ledger = "SL",
            string round = "TRUE");

        decimal GetExchangeRate(string fromCurrency, string toCurrency);
    }
}
