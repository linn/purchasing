namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Boms;

    public class BomReportsFacadeService : IBomReportsFacadeService
    {
        private readonly IBomReportsService domainService;

        private readonly IReportReturnResourceBuilder resultsModelResourceBuilder;

        public BomReportsFacadeService(
            IBomReportsService domainService,
            IReportReturnResourceBuilder resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IResult<ReportReturnResource> GetPartsOnBomReport(string bomName)
        {
            var resource = this.resultsModelResourceBuilder.Build(
                this.domainService.GetPartsOnBomReport(bomName.ToUpper().Trim()));

            return new SuccessResult<ReportReturnResource>(resource);
        }

        public IEnumerable<IEnumerable<string>> GetPartsOnBomExport(string bomName)
        {
            return this.domainService
                .GetPartsOnBomReport(bomName.ToUpper().Trim())
                .ConvertToCsvList();
        }
    }
}
