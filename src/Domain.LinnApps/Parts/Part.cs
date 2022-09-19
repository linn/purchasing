namespace Linn.Purchasing.Domain.LinnApps.Parts
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class Part
    {
        public string Description { get; set; }

        public int Id { get; set; }

        public string PartNumber { get; set; }

        public string StockControlled { get; set; }

        public string BomType { get; set; }

        public string LinnProduced { get; set; }

        public decimal? LabourPrice { get; set; }

        public decimal? MaterialPrice { get; set; }

        public decimal? BaseUnitPrice { get; set; }

        public decimal? CurrencyUnitPrice { get; set; }

        public Currency Currency { get; set; }

        public Supplier PreferredSupplier { get; set; }

        public IEnumerable<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        public string OurUnitOfMeasure { get; set; }

        public int? BomId { get; set; }

        public bool SupplierAssembly()
        {
            if (this.BomType == "A")
            {
                if (this.LinnProduced == "Y")
                {
                    return true;
                }

                if (this.PreferredSupplier?.SupplierId != 4415)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

