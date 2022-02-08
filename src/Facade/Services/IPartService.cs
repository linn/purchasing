namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;

    public interface IPartService
    {
        string GetPartNumberFromId(int id);

        IResult<PartPriceConversionsResource> GetPrices(
            string partNumber, 
            string newCurrency, 
            decimal newPrice, 
            string ledger, 
            string round);
    }
}
