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

        public decimal? TriggerBuild { get; set; }

        public decimal? PurchaseOrders { get; set; }

        public decimal? UnauthorisedPurchaseOrders { get; set; }

        public decimal? SalesOrders { get; set; }

        public decimal? DeliveryForecast { get; set; }

        public decimal? ProductionRequirement { get; set; }

        public string Status { get; set; }

        public decimal? Stock { get; set; }
  
        public decimal? MinRail { get; set; }

        public decimal? MaxRail { get; set; }

        public decimal? IdealStock { get; set; }
        
        public decimal? RecommendedOrders { get; set; }

        public decimal? RecommenedStock { get; set; }
    }
}
