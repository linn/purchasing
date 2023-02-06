namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public class BomStandardPriceFacadeService : IBomStandardPriceFacadeService
    {
        private readonly IBomStandardPriceService domainService;

        public BomStandardPriceFacadeService(IBomStandardPriceService domainService)
        {
            this.domainService = domainService;
        }

        public IResult<IEnumerable<BomStandardPrice>> GetData(string searchTerm)
        {
            return new SuccessResult<IEnumerable<BomStandardPrice>>(
                this.domainService.GetPriceVarianceInfo(searchTerm));
        }
    }
}
