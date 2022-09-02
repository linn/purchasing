namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    public class MonthlyForecastPartValues
    {
        public string PartNumber { get; set; }

        public int MonthEndWeek { get; set; }

        public string Usages { get; set; }

        public string ForecastOrders { get; set; }

        public string Stock { get; set; }

        public string Orders { get; set; }
    }
}
