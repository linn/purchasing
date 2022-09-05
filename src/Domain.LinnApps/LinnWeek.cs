namespace Linn.Purchasing.Domain.LinnApps
{
    using System;

    using Linn.Purchasing.Domain.LinnApps.Forecasting;

    public class LinnWeek
    {
        public int WeekNumber { get; set; }

        public DateTime StartsOn { get; set; }

        public DateTime EndsOn { get; set; }

        public ForecastWeekChange ForecastChange { get; set; }
    }
}
