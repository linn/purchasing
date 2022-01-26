namespace Linn.Purchasing.Proxy
{
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    public class AutocostPack : IAutocostPack
    {
        public decimal CalculateNewMaterialPrice(string partNumber, string newCurrency, decimal newCurrencyPrice)
        {
            throw new System.NotImplementedException();
        }
    }
}
