namespace Linn.Purchasing.Domain.LinnApps
{
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PartSpend
    {
        public decimal BaseTotal { get; set; }
        
        public int OrderLine { get; set; }

        public int OrderNumber { get; set; }

        public int LedgerPeriod { get; set; }
        
        public string PartNumber { get; set; }

        public string PartDescription { get; set; }
    }
}
