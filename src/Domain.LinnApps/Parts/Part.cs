namespace Linn.Purchasing.Domain.LinnApps.Parts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
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

        public string RawOrFinished { get; set; }

        public string DrawingReference { get; set; }

        public NominalAccount NominalAccount { get; set; }

        public string DecrementRule { get; set; }

        public IEnumerable<PartSupplier> PartSuppliers { get; set; }

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

        public bool ValidBomTypeChange(string newBomType)
        {
            // check valid new bom type
            var validBomTypes = new string[] {"C", "A", "P"};
            if (!validBomTypes.Contains(newBomType))
            {
                return false;
            }

            if (this.BomType == newBomType)
            {
                return false;  // not a change
            }

            return true;
        }

        public Part ClonePricingFields()
        {
            return new Part
                       {
                           PartNumber = this.PartNumber,
                           BomType = this.BomType,
                           PreferredSupplier = this.PreferredSupplier,
                           MaterialPrice = this.MaterialPrice,
                           LabourPrice = this.LabourPrice,
                           Currency = this.Currency,
                           CurrencyUnitPrice = this.CurrencyUnitPrice,
                           BaseUnitPrice = this.BaseUnitPrice
                       };
        }
    }
}

