namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    public class PartSpend
    {
        public decimal BaseTotal { get; set; }

        public int LedgerPeriod { get; set; }

        public int OrderLine { get; set; }

        public int OrderNumber { get; set; }

        public string PartNumber { get; set; }
    }
}
