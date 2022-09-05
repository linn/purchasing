namespace Linn.Purchasing.Domain.LinnApps.Forecasting
{
    public class ForecastWeekChange
    {
        public int LinnWeekNumber { get; set; }

        public LinnWeek LinnWeek { get; set; }

        public decimal PercentageChange { get; set; }
    }
}