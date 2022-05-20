namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Resources.RequestResources;

    public class ShortagesReportFacadeService : IShortagesReportFacadeService
    {
        private readonly IShortagesReportService domainService;

        private readonly IBuilder<ResultsModel> resultsModelResourceBuilder;

        public ShortagesReportFacadeService(
            IShortagesReportService domainService,
            IBuilder<ResultsModel> resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IResult<IEnumerable<ReportReturnResource>> GetReport(ShortagesReportRequestResource options)
        {
            var purchaseLevel = int.Parse(options.PurchaseLevel);
            var result = this.domainService.GetReport(
                purchaseLevel,
                options.VendorManager);

            var resultResources = result.Select(
                d => (ReportReturnResource)this.resultsModelResourceBuilder.Build(d, null));

            return new SuccessResult<IEnumerable<ReportReturnResource>>(resultResources);
        }
    }
}
