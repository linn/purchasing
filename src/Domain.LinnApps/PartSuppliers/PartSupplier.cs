namespace Linn.Purchasing.Domain.LinnApps.PartSuppliers
{
    using System;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PartSupplier
    {
        public string PartNumber { get; set; }

        public int SupplierId { get; set; }

        public Part Part { get; set; }

        public Supplier Supplier { get; set; }

        public string SupplierDesignation { get; set; }

        public OrderMethod OrderMethod { get; set; }

        public Currency Currency { get; set; }

        public decimal? CurrencyUnitPrice { get; set; }

        public decimal? OurCurrencyPriceToShowOnOrder { get; set; }

        public decimal? BaseOurUnitPrice { get; set; }

        public decimal MinimumOrderQty { get; set; }

        public decimal? MinimumDeliveryQty { get; set; }

        public decimal? ReelOrBoxQty { get; set; }

        public decimal OrderIncrement { get; set; }

        public string UnitOfMeasure { get; set; }

        public int LeadTimeWeeks { get; set; }

        public string OverbookingAllowed { get; set; }

        public decimal? DamagesPercent { get; set; }

        public string DeliveryInstructions { get; set; }

        public string NotesForBuyer { get; set; }

        public int? JitReorderWeeks { get; set; }

        public int? JitBookinOrderNumber { get; set; }

        public int? JitBookinOrderLine { get; set; }

        public int? JitReorderOrderNumber { get; set; }

        public int? JitReorderOrderLine { get; set; }

        public string JitStatus { get; set; }

        public Employee CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public Employee MadeInvalidBy { get; set; }

        public DateTime? DateInvalid { get; set; }

        public Manufacturer Manufacturer { get; set; }

        public string ManufacturerPartNumber { get; set; }

        public string VendorPartNumber { get; set; }

        public FullAddress DeliveryFullAddress { get; set; }

        public string PackWasteStatus { get; set; }

        public int? SupplierRanking { get; set; }
    }
}
