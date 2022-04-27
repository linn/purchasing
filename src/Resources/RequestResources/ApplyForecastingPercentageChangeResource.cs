namespace Linn.Purchasing.Resources.RequestResources
{
    public class ApplyForecastingPercentageChangeResource
    {
        public decimal Change { get; set; }

        public int StartPeriod { get; set; }

        public int EndPeriod { get; set; }
    }
}
