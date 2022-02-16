namespace Linn.Purchasing.Resources
{
    using Linn.Common.Resources;

    public class PartSupplierResource : HypermediaResource
    {
        public string PartNumber { get; set; }

        public string PartDescription { get; set; }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public int? PackingGroup { get; set; }

        public string PackingGroupDescription { get; set; }

        public int? CreatedBy { get; set; }

        public string CreatedByName { get; set; }

        public string OrderMethodName { get; set; }

        public string OrderMethodDescription { get; set; }

        public string ManufacturerCode { get; set; }

        public string ManufacturerName { get; set; }

        public int? AddressId { get; set; }

        public string FullAddress { get; set; }

        public string Designation { get; set; }

        public string Stream { get; set; }

        public string CurrencyCode { get; set; }

        public string UnitOfMeasure { get; set; }

        public decimal? CurrencyUnitPrice { get; set; }

        public decimal? OurCurrencyPriceToShowOnOrder { get; set; }

        public decimal? BaseOurUnitPrice { get; set; }

        public decimal MinimumOrderQty { get; set; }

        public decimal? MinimumDeliveryQty { get; set; }

        public decimal OrderIncrement { get; set; }

        public decimal? OrderConversionFactor { get; set; }

        public decimal? ReelOrBoxQty { get; set; }

        public int LeadTimeWeeks { get; set; }

        public string OverbookingAllowed { get; set; }

        public decimal? DamagesPercent { get; set; }

        public string WebAddress { get; set; }

        public string DeliveryInstructions { get; set; }

        public string NotesForBuyer { get; set; }

        public string PackWasteStatus { get; set; }

        public int? PackagingGroupId{ get; set; }

        public string PackagingGroupDescription { get; set; }

        public string DateCreated { get; set; }

        public string DateInvalid { get; set; }

        public int? MadeInvalidBy { get; set; }

        public string ManufacturerPartNumber { get; set; }

        public string VendorPartNumber { get; set; }

        public int? SupplierRanking { get; set; }
    }
}
