namespace Linn.Purchasing.Resources.RequestResources
{
    public class ApplyForecastingPercentageChangeResource
    {
        public decimal Change { get; set; }

        public int StartMonth { get; set; }

        public int StartYear { get; set; }


        public int EndMonth { get; set; }

        public int EndYear { get; set; }
    }
}
