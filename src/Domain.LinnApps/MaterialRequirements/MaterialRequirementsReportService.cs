namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;

    public class MaterialRequirementsReportService : IMaterialRequirementsReportService
    {
        private readonly IQueryRepository<MrHeader> repository;

        public MaterialRequirementsReportService(IQueryRepository<MrHeader> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<MrHeader> GetMaterialRequirements(string jobRef, IEnumerable<string> partNumbers)
        {
            var results = this.repository.FilterBy(a => a.JobRef == jobRef && partNumbers.Contains(a.PartNumber)).ToList();
            return results;
        }
    }
}
