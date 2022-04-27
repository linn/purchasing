namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Forecasting;
    using Linn.Purchasing.Resources;

    public class ForecastingFacadeService : IForecastingFacadeService
    {
        private readonly IForecastingService domainService;

        public ForecastingFacadeService(IForecastingService domainService)
        {
            this.domainService = domainService;
        }

        public IResult<ProcessResultResource> ApplyPercentageChange(
            decimal change, int startPeriod, int endPeriod, IEnumerable<string> privileges)
        {
            var result = this.domainService.ApplyPercentageChange(change, startPeriod, endPeriod, privileges);
            return new SuccessResult<ProcessResultResource>(
                new ProcessResultResource
                    {
                        Message = result.Message,
                        Success = result.Success
                    });
        }
    }
}
