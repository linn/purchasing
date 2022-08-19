namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    public class SuppliersLeadTimesEntry
    {
        public string PartNumber { get; set; }

        public int SupplierId { get; set; }

        public int LeadTimeWeeks { get; set; }
    }
}
