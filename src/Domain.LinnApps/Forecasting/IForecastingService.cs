namespace Linn.Purchasing.Domain.LinnApps.Forecasting
{
    using System.Collections.Generic;

    public interface IForecastingService
    {
        ProcessResult ApplyPercentageChange(
            decimal change, int startPeriod, int endPeriod, IEnumerable<string> privileges);
    }
}
