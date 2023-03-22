namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Reports;

    public class ChangeStatusReportFacadeService : IChangeStatusReportsFacadeService
    {
        private readonly IChangeStatusReportService domainService;

        private readonly IReportReturnResourceBuilder reportReturnResourceBuilder;

        public ChangeStatusReportFacadeService(
            IChangeStatusReportService domainService,
            IReportReturnResourceBuilder reportReturnResourceBuilder)
        {
            this.domainService = domainService;
            this.reportReturnResourceBuilder = reportReturnResourceBuilder;
        }

        public IResult<ReportReturnResource> GetChangeStatusReport(int months)
        {
            var resource = this.reportReturnResourceBuilder.Build(
                this.domainService.GetChangeStatusReport(months));

            return new SuccessResult<ReportReturnResource>(resource);
        }

        public IResult<ReportReturnResource> GetAcceptedChangesReport(int months)
        {
            var resource = this.reportReturnResourceBuilder.Build(
                this.domainService.GetAcceptedChangesReport(months));

            return new SuccessResult<ReportReturnResource>(resource);
        }

        public IResult<ReportReturnResource> GetProposedChangesReport(int months)
        {
            var resource = this.reportReturnResourceBuilder.Build(
                this.domainService.GetProposedChangesReport(months));

            return new SuccessResult<ReportReturnResource>(resource);
        }

        public IResult<ReportReturnResource> GetTotalOutstandingChangesReport(int months)
        {
            var resource = this.reportReturnResourceBuilder.Build(
                this.domainService.GetTotalOutstandingChangesReport(months));

            return new SuccessResult<ReportReturnResource>(resource);
        }
    }
}
