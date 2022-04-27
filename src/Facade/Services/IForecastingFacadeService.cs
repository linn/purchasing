namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;

    public interface IForecastingFacadeService
    {
        IResult<ProcessResultResource> ApplyPercentageChange(
            decimal change, 
            int startPeriod, 
            int endPeriod, 
            IEnumerable<string> privileges);
    }
}
