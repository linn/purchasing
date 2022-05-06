namespace Linn.Purchasing.Resources.MaterialRequirements
{
    public class MrDetailResource
    {
        public string JobRef { get; set; }

        public string PartNumber { get; set; }

        public int LinnWeekNumber { get; set; }

        public decimal? DeliveryForecast { get; set; }

        public decimal? ProductionRequirement { get; set; }
    }
}
