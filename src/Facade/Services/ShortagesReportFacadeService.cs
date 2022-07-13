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

        public IResult<ReportReturnResource> GetShortagesReport(ShortagesReportRequestResource options)
        {
            var purchaseLevel = int.Parse(options.PurchaseLevel);
            var resource = this.resultsModelResourceBuilder.Build(
                this.domainService.GetShortagesReport(purchaseLevel, options.VendorManager));

            return new SuccessResult<ReportReturnResource>(resource);
        }

        public IResult<ReportReturnResource> GetShortagesPlannerReport(int planner)
        {
            var resource = this.resultsModelResourceBuilder.Build(
                this.domainService.GetShortagesPlannerReport(planner));

            return new SuccessResult<ReportReturnResource>(resource);
        }
    }
}
