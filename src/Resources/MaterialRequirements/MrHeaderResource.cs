namespace Linn.Purchasing.Resources.MaterialRequirements
{
    using System.Collections.Generic;

    using Linn.Common.Resources;

    public class MrHeaderResource : HypermediaResource
    {
        public string JobRef { get; set; }

        public string PartNumber { get; set; }

        public string PartDescription { get; set; }

        public decimal QuantityInStock { get; set; }

        public decimal QuantityForSpares { get; set; }

        public decimal QuantityInInspection { get; set; }

        public decimal QuantityFaulty { get; set; }

        public decimal QuantityAtSupplier { get; set; }

        public int? PreferredSupplierId { get; set; }

        public string PreferredSupplierName { get; set; }

        public decimal AnnualUsage { get; set; }

        public decimal? BaseUnitPrice { get; set; }

        public string OurUnits { get; set; }

        public string OrderUnits { get; set; }

        public int? LeadTimeWeeks { get; set; }

        public string CurrencyCode { get; set; }

        public decimal? CurrencyUnitPrice { get; set; }

        public decimal? MinimumOrderQuantity { get; set; }

        public decimal? MinimumDeliveryQuantity { get; set; }

        public decimal? OrderIncrement { get; set; }

        public string VendorManager { get; set; }

        public string VendorManagerInitials { get; set; }

        public int? Planner { get; set; }
        
        public string MrComments { get; set; }

        public decimal? RecommendedOrderQuantity { get; set; }

        public string RecommendedOrderDate { get; set; }

        public IEnumerable<MrDetailResource> Details { get; set; }
    }
}
