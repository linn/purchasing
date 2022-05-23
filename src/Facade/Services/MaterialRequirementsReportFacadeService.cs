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

        private readonly IBuilder<MrReportOptions> optionsBuilder;

        public MaterialRequirementsReportFacadeService(
            IMaterialRequirementsReportService materialRequirementsReportService,
            IBuilder<MrReport> builder,
            IBuilder<MrReportOptions> optionsBuilder)
        {
            this.materialRequirementsReportService = materialRequirementsReportService;
            this.builder = builder;
            this.optionsBuilder = optionsBuilder;
        }

        public IResult<MrReportResource> GetMaterialRequirements(MrRequestResource request, IEnumerable<string> privileges)
        {
            var parts = string.IsNullOrEmpty(request.PartNumber)
                            ? request.PartNumbers
                            : new List<string> { request.PartNumber };

            var report = this.materialRequirementsReportService.GetMaterialRequirements(
                request.JobRef,
                request.TypeOfReport,
                request.PartSelector,
                request.StockLevelSelector,
                parts);

            return new SuccessResult<MrReportResource>((MrReportResource)this.builder.Build(report, privileges));
        }

        public IResult<MrReportOptionsResource> GetOptions(IEnumerable<string> privileges)
        {
            var reportOptions = this.materialRequirementsReportService.GetOptions();
            return new SuccessResult<MrReportOptionsResource>((MrReportOptionsResource)this.optionsBuilder.Build(reportOptions, privileges));
        }
    }
}
