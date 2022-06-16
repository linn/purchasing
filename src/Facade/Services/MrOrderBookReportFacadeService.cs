namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Domain.LinnApps.Reports;

    public class MrOrderBookReportFacadeService : IMrOrderBookReportFacadeService
    {
        private readonly IMrOrderBookReportService domainService;

        private readonly IBuilder<IEnumerable<ResultsModel>> resultsModelResourceBuilder;

        public MrOrderBookReportFacadeService(
            IMrOrderBookReportService domainService,
            IBuilder<IEnumerable<ResultsModel>> resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IResult<ReportReturnResource> GetReport(int supplierId)
        {
            var resource = (ReportReturnResource)this.resultsModelResourceBuilder.Build(
                new List<ResultsModel> {this.domainService.GetOrderBookReport(supplierId) }, null);
            
            return new SuccessResult<ReportReturnResource>(resource);
        }
    }
}
