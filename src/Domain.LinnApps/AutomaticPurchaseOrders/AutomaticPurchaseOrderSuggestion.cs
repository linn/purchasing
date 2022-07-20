namespace Linn.Purchasing.Domain.LinnApps.AutomaticPurchaseOrders
{
    using System;

    public class AutomaticPurchaseOrderSuggestion
    {
        public string PartNumber { get; set; }

        public int PreferredSupplierId { get; set; }

        public decimal RecommendedQuantity { get; set; }

        public DateTime RecommendedDate { get; set; }

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
