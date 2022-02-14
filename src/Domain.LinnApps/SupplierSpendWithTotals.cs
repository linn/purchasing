namespace Linn.Purchasing.Domain.LinnApps
{
    public class SupplierSpendWithTotals : SupplierSpend
    {
        public decimal MonthTotal { get; set; }

        public decimal PrevYearTotal { get; set; }

        public decimal YearTotal { get; set; }
    }
}
