namespace Linn.Purchasing.Domain.LinnApps.PartSuppliers
{
    using System;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PreferredSupplierChange
    {
        public string PartNumber { get; set; }

        public int Seq { get; set; }

        public Supplier OldSupplier { get; set; }

        public Supplier NewSupplier { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? NewPrice { get; set; }

        public Currency NewCurrency { get; set; }

        public DateTime DateChanged { get; set; }

        public Employee ChangedBy { get; set; }

        public string Remarks { get; set; }

        public PriceChangeReason ChangeReason { get; set; }
    }
}
