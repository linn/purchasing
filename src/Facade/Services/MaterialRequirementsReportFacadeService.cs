namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    public class MaterialRequirementsReportFacadeService : IMaterialRequirementsReportFacadeService
    {
        private readonly IMaterialRequirementsReportService materialRequirementsReportService;

        private readonly IBuilder<MrReport> builder;

        public MaterialRequirementsReportFacadeService(
            IMaterialRequirementsReportService materialRequirementsReportService,
            IBuilder<MrReport> builder)
        {
            this.materialRequirementsReportService = materialRequirementsReportService;
            this.builder = builder;
        }

        public IResult<MrReportResource> GetMaterialRequirements(MrRequestResource request, IEnumerable<string> privileges)
        {
            var parts = string.IsNullOrEmpty(request.PartNumber)
                            ? request.PartNumbers
                            : new List<string> { request.PartNumber };

            var report = this.materialRequirementsReportService.GetMaterialRequirements(request.JobRef, parts);

            return new SuccessResult<MrReportResource>((MrReportResource)this.builder.Build(report, privileges));
        }
    }
}
