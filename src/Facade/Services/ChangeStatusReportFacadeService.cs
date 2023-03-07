namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Resources.RequestResources;

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

        public IResult<ReportReturnResource> GetChangeStatusReport(ChangeStatusReportRequestResource options)
        {
            var resource = this.reportReturnResourceBuilder.Build(
                this.domainService.GetChangeStatusReport(int.Parse(options.Months)));

            return new SuccessResult<ReportReturnResource>(resource);
        }

        public IResult<ReportReturnResource> GetAcceptedChangesReport(ChangeStatusReportRequestResource options)
        {
            var resource = this.reportReturnResourceBuilder.Build(
                this.domainService.GetAcceptedChangesReport(int.Parse(options.Months)));

            return new SuccessResult<ReportReturnResource>(resource);
        }

        public IResult<ReportReturnResource> GetProposedChangesReport(ChangeStatusReportRequestResource options)
        {
            var resource = this.reportReturnResourceBuilder.Build(
                this.domainService.GetProposedChangesReport(int.Parse(options.Months)));

            return new SuccessResult<ReportReturnResource>(resource);
        }

        public IResult<ReportReturnResource> GetTotalOutstandingChangesReport(ChangeStatusReportRequestResource options)
        {
            var resource = this.reportReturnResourceBuilder.Build(
                this.domainService.GetTotalOutstandingChangesReport(int.Parse(options.Months)));

            return new SuccessResult<ReportReturnResource>(resource);
        }
    }
}
