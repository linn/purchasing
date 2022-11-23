namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using Linn.Purchasing.Domain.LinnApps.Boms;

    public class PartRequirement
    {
        public string PartNumber { get; set; }

        public decimal AnnualUsage { get; set; }

        public BomDetail BomDetail { get; set; }
    }
}
