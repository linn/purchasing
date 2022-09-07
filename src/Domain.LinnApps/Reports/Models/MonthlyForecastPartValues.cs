namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    public class MonthlyForecastPartValues
    {
        public string PartNumber { get; set; }

        public int MonthEndWeek { get; set; }

        public decimal? Usages { get; set; }

        public decimal? ForecastOrders { get; set; }

        public decimal? Stock { get; set; }

        public decimal? Orders { get; set; }
    }
}
