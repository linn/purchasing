namespace Linn.Purchasing.Domain.LinnApps
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class SupplierSpend
    {
        public int PlTref { get; }

        public decimal BaseTotal { get; set; }

        public int LedgerPeriod { get; set; }

        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; }
        [NotMapped]
        public decimal MonthTotal { get; set; }
        [NotMapped]
        public decimal YearTotal { get; set; }
        [NotMapped]
        public decimal PrevYearTotal { get; set; }
    }
}
