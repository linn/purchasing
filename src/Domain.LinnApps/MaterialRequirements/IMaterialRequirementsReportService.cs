namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System.Collections.Generic;

    public interface IMaterialRequirementsReportService
    {
        public IEnumerable<MrHeader> GetMaterialRequirements(string jobRef, IEnumerable<string> partNumbers);
    }
}
