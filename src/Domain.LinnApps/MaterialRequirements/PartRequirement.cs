namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class PartRequirement
    {
        public string PartNumber { get; set; }

        public decimal AnnualUsage { get; set; }

        public BomDetailViewEntry BomDetailViewEntry { get; set; }
        
        public Part Part { get; set; }
    }
}
