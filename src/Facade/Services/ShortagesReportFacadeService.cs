namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Resources.RequestResources;

    public class ShortagesReportFacadeService : IShortagesReportFacadeService 
    {
        private readonly IShortagesReportService domainService;

        private readonly IReportReturnResourceBuilder resultsModelResourceBuilder;

        public ShortagesReportFacadeService(
            IShortagesReportService domainService,
            IReportReturnResourceBuilder resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IResult<IEnumerable<ReportReturnResource>> GetShortagesReport(ShortagesReportRequestResource options)
        {
            var purchaseLevel = int.Parse(options.PurchaseLevel);
            var result = this.domainService.GetShortagesReport(
                purchaseLevel,
                options.VendorManager);

            var resultResources = result.Select(
                d => this.resultsModelResourceBuilder.Build(d));

            return new SuccessResult<IEnumerable<ReportReturnResource>>(resultResources);
        }

        public IResult<IEnumerable<ReportReturnResource>> GetShortagesPlannerReport(int planner)
        {
            var result = this.domainService.GetShortagesPlannerReport(planner);
            var resultResources = result.Select(
                d => (ReportReturnResource)this.resultsModelResourceBuilder.Build(d, null));

            return new SuccessResult<IEnumerable<ReportReturnResource>>(resultResources);
        }
    }
}
