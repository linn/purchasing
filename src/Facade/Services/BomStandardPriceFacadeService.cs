namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Domain.Exceptions;
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

        public IResult<BomStandardPricesResource> DoUpdate(BomStandardPricesResource resource)
        {
            SetStandardPriceResult result;
            try
            {
                result = this.domainService.SetStandardPrices(
                    resource.Lines,
                    resource.UpdatedBy.GetValueOrDefault(),
                    resource.Remarks);
            }
            catch (DomainException ex)
            {
                return new BadRequestResult<BomStandardPricesResource>(ex.Message);
            }

            return new SuccessResult<BomStandardPricesResource>(
                new BomStandardPricesResource
                    {
                        Lines = result.Lines, ReqNumber = result.ReqNumber, Message = result.Message
                    });
        }
    }
}
