namespace Linn.Purchasing.Domain.LinnApps
{
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class SupplierSpend
    {
        public int PlTref { get; }

        public decimal BaseTotal { get; set; }

        public int LedgerPeriod { get; set; }

        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; }

        public decimal MonthTotal { get; set; }

        public decimal YearTotal { get; set; }

        public decimal PrevYearTotal { get; set; }

    }
}
