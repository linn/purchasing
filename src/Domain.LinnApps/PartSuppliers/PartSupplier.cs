namespace Linn.Purchasing.Domain.LinnApps.PartSuppliers
{
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PartSupplier
    {
        public string PartNumber { get; set; }

        public int SupplierId { get; set; }

        public Part Part { get; set; }

        public Supplier Supplier { get; set; }

        public string SupplierDesignation { get; set; }

        public OrderMethod OrderMethod { get; set; }

        public string Currency { get; set; }

        public decimal CurrencyUnitPrice { get; set; }

        public decimal OurCurrencyPriceToShowOnOrder { get; set; }

        public decimal BaseOurUnitPrice { get; set; }

        public decimal MinimumOrderQty { get; set; }

        public decimal MinimumDeliverQty { get; set; }

        public decimal ReelOrBoxQty { get; set; }

        public decimal OrderIncrement { get; set; }

        public string UnitOfMeasure { get; set; }

        public decimal OrderConversionFactory { get; set; }
    }
}
