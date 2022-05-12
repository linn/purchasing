namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    public class MrDetail
    {
        public string JobRef { get; set; }

        public string PartNumber { get; set; }

        public int LinnWeekNumber { get; set; }

        public int Segment { get; set; }

        public decimal? DeliveryForecast { get; set; }

        public decimal? ProductionRequirement { get; set; }
    }
}
