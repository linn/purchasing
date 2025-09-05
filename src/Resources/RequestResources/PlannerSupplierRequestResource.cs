namespace Linn.Purchasing.Resources.RequestResources
{
    public class PlannerSupplierRequestResource
    {
        public int? SupplierId { get; set; }
        
        public int? Planner { get; set; }

        public decimal? MinimumRecommendedQuantity { get; set; } = 1;
    }
}
