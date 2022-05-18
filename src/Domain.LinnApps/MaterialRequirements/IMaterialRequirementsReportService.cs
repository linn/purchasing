namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System.Collections.Generic;

    public interface IMaterialRequirementsReportService
    {
        public MrReport GetMaterialRequirements(string jobRef, IEnumerable<string> partNumbers);
    }
}
