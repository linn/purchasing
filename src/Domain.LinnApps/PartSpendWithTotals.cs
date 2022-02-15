namespace Linn.Purchasing.Domain.LinnApps
{
    public class PartSpendWithTotals
    {
        public decimal BaseTotal { get; set; }

        public int LedgerPeriod { get; set; }

        public decimal MonthTotal { get; set; }

        public int OrderLine { get; set; }

        public int OrderNumber { get; set; }

        public string PartDescription { get; set; }

        public string PartNumber { get; set; }

        public decimal PrevYearTotal { get; set; }

        public decimal YearTotal { get; set; }
    }
}
