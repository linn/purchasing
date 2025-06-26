namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public interface IForecastingFacadeService
    {
        IResult<ProcessResultResource> ApplyPercentageChange(
            ApplyForecastingPercentageChangeResource resource,
            IEnumerable<string> privileges);
    }
}
