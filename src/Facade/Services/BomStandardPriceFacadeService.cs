namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Resources.Boms;

    public class BomStandardPriceFacadeService : IBomStandardPriceFacadeService
    {
        private readonly IBomStandardPriceService domainService;

        public BomStandardPriceFacadeService(IBomStandardPriceService domainService)
        {
            this.domainService = domainService;
        }

        public IResult<BomStandardPricesResource> GetData(string searchTerm)
        {
            return new SuccessResult<BomStandardPricesResource>(
                new BomStandardPricesResource
                    {
                        Lines = this.domainService.GetPriceVarianceInfo(searchTerm)
                    });
        }
    }
}
