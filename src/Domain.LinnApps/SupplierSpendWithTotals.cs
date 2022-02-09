namespace Linn.Purchasing.Domain.LinnApps
{
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class SupplierSpendWithTotals
    {
        public decimal BaseTotal { get; set; }

        public int LedgerPeriod { get; set; }

        public decimal MonthTotal { get; set; }

        public int PlTref { get; }

        public decimal PrevYearTotal { get; set; }

        public Supplier Supplier { get; set; }

        public int SupplierId { get; set; }

        public decimal YearTotal { get; set; }
    }
}
