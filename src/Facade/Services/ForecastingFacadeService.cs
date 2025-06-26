namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.Forecasting;
    using Linn.Purchasing.Resources.RequestResources;

    public class ForecastingFacadeService : IForecastingFacadeService
    {
        private readonly IForecastingService domainService;

        public ForecastingFacadeService(IForecastingService domainService)
        {
            this.domainService = domainService;
        }

        public IResult<ProcessResultResource> ApplyPercentageChange(
            ApplyForecastingPercentageChangeResource resource, IEnumerable<string> privileges)
        {
            var result = this.domainService.ApplyPercentageChange(
                resource.Change, 
                resource.StartMonth, 
                resource.StartYear, 
                resource.EndMonth, 
                resource.EndYear, 
                privileges);

            return new SuccessResult<ProcessResultResource>(
                new ProcessResultResource
                    {
                        Message = result.Message,
                        Success = result.Success
                    });
        }
    }
}
