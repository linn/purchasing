namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface IAutocostPack
    {
        decimal CalculateNewMaterialPrice(string partNumber, string newCurrency, decimal newCurrencyPrice);
    }
}
