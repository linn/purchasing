namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface ICurrencyPack
    {
        decimal CalculateBaseValueFromCurrencyValue(string newCurrency, decimal newPrice);
    }
}
