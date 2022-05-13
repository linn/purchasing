﻿namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System.Collections.Generic;

    public class MrHeader
    {
        public string JobRef { get; set; }

        public string PartNumber { get; set; }

        public string PartDescription { get; set; }

        public decimal QuantityInStock { get; set; }

        public decimal QuantityForSpares { get; set; }

        public decimal QuantityInInspection { get; set; }

        public decimal QuantityFaulty { get; set; }

        public decimal QuantityAtSupplier { get; set; }

        public int PreferredSupplierId { get; set; }

        public string PreferredSupplierName { get; set; }

        public decimal AnnualUsage { get; set; }

        public decimal BaseUnitPrice { get; set; }

        public string OurUnits { get; set; }

        public string OrderUnits { get; set; }

        public int LeadTimeWeeks { get; set; }

        public string CurrencyCode { get; set; }

        public decimal CurrencyUnitPrice { get; set; }

        public decimal? MinimumOrderQuantity { get; set; }

        public decimal? MinimumDeliveryQuantity { get; set; }

        public decimal? OrderIncrement { get; set; }

        public string HasProductionRequirement { get; set; }
        
        public string HasDeliveryForecast { get; set; }
        
        public string HasSalesOrders { get; set; }

        public string HasPurchaseOrders { get; set; }

        public string HasUnauthPurchaseOrders { get; set; }
        
        public string HasTriggerBuild { get; set; }

        public string VendorManager { get; set; }

        public string VendorManagerInitials { get; set; }

        public int PartId { get; set; }

        public IEnumerable<MrDetail> MrDetails { get; set; }
    }
}
