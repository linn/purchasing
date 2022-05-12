namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    public class MrDetail
    {
        public string JobRef { get; set; }

        public string PartNumber { get; set; }

        public int LinnWeekNumber { get; set; }

        public string WeekEnding { get; set; }

        public int Segment { get; set; }

        public string WeekAndYear { get; set; }

        public decimal? DeliveryForecast { get; set; }

        public decimal? ProductionRequirement { get; set; }

        public decimal? Stock { get; set; }

        public decimal? MinRail { get; set; }

        public decimal? MaxRail { get; set; }
    }
}
