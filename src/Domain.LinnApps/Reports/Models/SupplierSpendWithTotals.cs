namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    public class SupplierSpendWithTotals : SupplierSpend
    {
        public decimal MonthTotal { get; set; }

        public decimal PrevYearTotal { get; set; }

        public decimal YearTotal { get; set; }

        public decimal DateRangeTotal { get; set; }
    }
}
