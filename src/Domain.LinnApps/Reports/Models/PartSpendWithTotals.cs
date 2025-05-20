namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    public class PartSpendWithTotals : PartSpend
    {
        public decimal MonthTotal { get; set; }

        public string PartDescription { get; set; }

        public decimal YearBeforeLastTotal { get; set; }

        public decimal PrevYearTotal { get; set; }

        public decimal YearTotal { get; set; }
    }
}
