namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;

    public interface IPartFacadeService
    {
        string GetPartNumberFromId(int id);

        IResult<PartPriceConversionsResource> GetPrices(
            string partNumber, 
            string newCurrency, 
            decimal newPrice, 
            string ledger, 
            string round);

        IResult<BomTypeChangeResource> ChangeBomType(
            BomTypeChangeResource request, IEnumerable<string> privileges = null);
    }
}
