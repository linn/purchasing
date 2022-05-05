namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    public class MrUsedOnRecord
    {
        public string PartNumber { get; set; }

        public string Description { get; set; }

        public string JobRef { get; set; }

        public string AssemblyUsedOn { get; set; }

        public string AssemblyUsedOnDescription { get; set; }

        public decimal QtyUsed { get; set; }

        public string TCoded { get; set; }

        public decimal AnnualUsage { get; set; }
    }
}
