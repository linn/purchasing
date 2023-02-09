namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Resources.Boms;

    public interface IBomStandardPriceFacadeService
    {
        IResult<BomStandardPricesResource> GetData(string searchTerm);

        IResult<BomStandardPricesResource> DoUpdate(BomStandardPricesResource resource);
    }
}
