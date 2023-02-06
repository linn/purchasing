namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface IAutocostPack
    {
        decimal CalculateNewMaterialPrice(
            string partNumber, string newCurrency, decimal newCurrencyPrice);

        void AutoCostAssembly(
            string partNumber,
            string changeType,
            int changedBy,
            string remarks);
    }
}
