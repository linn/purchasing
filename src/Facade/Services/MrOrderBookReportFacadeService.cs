namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Facade.ResourceBuilders;

    public class MrOrderBookReportFacadeService : IMrOrderBookReportFacadeService
    {
        private readonly IMrOrderBookReportService domainService;

        private readonly IReportReturnResourceBuilder resultsModelResourceBuilder;

        public MrOrderBookReportFacadeService(
            IMrOrderBookReportService domainService,
            IReportReturnResourceBuilder resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IResult<ReportReturnResource> GetReport(int supplierId)
        {
            var resource = (ReportReturnResource)this.resultsModelResourceBuilder.Build(
                new List<ResultsModel> {this.domainService.GetOrderBookReport(supplierId) });
            
            return new SuccessResult<ReportReturnResource>(resource);
        }
    }
}
