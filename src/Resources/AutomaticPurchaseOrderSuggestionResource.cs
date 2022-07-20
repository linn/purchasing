namespace Linn.Purchasing.Resources
{
    public class AutomaticPurchaseOrderSuggestionResource
    {
        public string PartNumber { get; set; }

        public int PreferredSupplierId { get; set; }

        public decimal RecommendedQuantity { get; set; }

        public string RecommendedDate { get; set; }

        public string RecommendationCode { get; set; }

        public string CurrencyCode { get; set; }

        public decimal OurPrice { get; set; }

        public string SupplierName { get; set; }

        public string OrderMethod { get; set; }

        public int? JitReorderNumber { get; set; }

        public string VendorManager { get; set; }
        
        public int? Planner { get; set; }
        
        public string JobRef { get; set; }
    }
}
