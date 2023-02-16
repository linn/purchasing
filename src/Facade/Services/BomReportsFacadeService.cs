namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public class BomReportsFacadeService : IBomReportsFacadeService
    {
        private readonly IBomReportsService domainService;

        private readonly IReportReturnResourceBuilder reportReturnResourceBuilder;

        public BomReportsFacadeService(
            IBomReportsService domainService,
            IReportReturnResourceBuilder reportReturnResourceBuilder)
        {
            this.domainService = domainService;
            this.reportReturnResourceBuilder = reportReturnResourceBuilder;
        }

        public IResult<ReportReturnResource> GetPartsOnBomReport(string bomName)
        {
            var resource = this.reportReturnResourceBuilder.Build(
                this.domainService.GetPartsOnBomReport(bomName.ToUpper().Trim()));

            return new SuccessResult<ReportReturnResource>(resource);
        }

        public IEnumerable<IEnumerable<string>> GetPartsOnBomExport(string bomName)
        {
            return this.domainService
                .GetPartsOnBomReport(bomName.ToUpper().Trim())
                .ConvertToCsvList();
        }

        public IResult<IEnumerable<BomCostReportResource>> GetBomCostReport(
            string bomName, bool splitBySubAssembly, int levels, decimal labourHourlyRate)
        {
            var result = this.domainService
                .GetBomCostReport(bomName.Trim().ToUpper(), splitBySubAssembly, levels, labourHourlyRate)
                .Select(r => new BomCostReportResource
                                          {
                                              Breakdown = this.reportReturnResourceBuilder.Build(r.Breakdown),
                                              SubAssembly = r.SubAssembly,
                                              MaterialTotal = r.MaterialTotal,
                                              StandardTotal = r.StandardTotal
                                          });
            return new SuccessResult<IEnumerable<BomCostReportResource>>(result);
        }

        public IResult<ReportReturnResource> GetBoardDifferenceReport(
            BomDifferenceReportRequestResource resource)
        {
            var result = this.domainService.GetBoardDifferenceReport(
                resource.BoardCode1,
                resource.RevisionCode1,
                resource.BoardCode2,
                resource.RevisionCode2);

            return new SuccessResult<ReportReturnResource>(this.reportReturnResourceBuilder.Build(result));
        }
    }
}
