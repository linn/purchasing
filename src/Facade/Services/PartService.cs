namespace Linn.Purchasing.Facade.Services
{
    using System;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Resources;

    public class PartService : IPartService
    {
        private readonly IQueryRepository<Part> partRepository;

        private readonly IAutocostPack autocostPack;

        private readonly ICurrencyPack currencyPack;

        public PartService(
            IQueryRepository<Part> partRepository, 
            IAutocostPack autocostPack, 
            ICurrencyPack currencyPack)
        {
            this.partRepository = partRepository;
            this.autocostPack = autocostPack;
            this.currencyPack = currencyPack;
        }

        public string GetPartNumberFromId(int id)
        {
            return this.partRepository.FindBy(x => x.Id == id)?.PartNumber;
        }

        public IResult<PartPriceConversionsResource> GetPrices(string partNumber, string newCurrency, decimal newPrice)
        {
            try
            {
                return new SuccessResult<PartPriceConversionsResource>(
                    new PartPriceConversionsResource
                        {
                            NewPrice = this.autocostPack.CalculateNewMaterialPrice(partNumber, newCurrency, newPrice),
                            BaseNewPrice = this.currencyPack.CalculateBaseValueFromCurrencyValue(newCurrency, newPrice)
                        });
            }
            catch (Exception e)
            {
                return new BadRequestResult<PartPriceConversionsResource>(e.Message);
            }
        }
    }
}
