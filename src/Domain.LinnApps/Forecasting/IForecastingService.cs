namespace Linn.Purchasing.Domain.LinnApps.Forecasting
{
    using System.Collections.Generic;

    public interface IForecastingService
    {
        ProcessResult ApplyPercentageChange(
            decimal change, 
            int startMonth, 
            int startYear, 
            int endMonth, 
            int endYear, 
            IEnumerable<string> privileges);
    }
}
