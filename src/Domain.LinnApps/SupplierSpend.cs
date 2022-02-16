namespace Linn.Purchasing.Domain.LinnApps
{
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class SupplierSpend
    {
        public decimal? BaseTotal { get; set; }

        public int LedgerPeriod { get; set; }

        public int? OrderLine { get; set; }

        public int? OrderNumber { get; set; }

        public int PlTref { get; }

        public Supplier Supplier { get; set; }

        public int SupplierId { get; set; }
    }
}
