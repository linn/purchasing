namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Facade.ResourceBuilders;

    public class SpendsReportFacadeService : ISpendsReportFacadeService
    {
        private readonly ISpendsReportService domainService;

        private readonly IReportReturnResourceBuilder resultsModelResourceBuilder;

        public SpendsReportFacadeService(
            ISpendsReportService domainService,
            IReportReturnResourceBuilder resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IEnumerable<IEnumerable<string>> GetSpendBySupplierExport(string vendorManagerId)
        {
            return this.domainService.GetSpendBySupplierReport(vendorManagerId).ConvertToCsvList();
        }

        public IResult<ReportReturnResource> GetSpendBySupplierReport(string vendorManagerId)
        {
            var results = this.domainService.GetSpendBySupplierReport(vendorManagerId);

            var returnResource = this.BuildResource(results);

            return new SuccessResult<ReportReturnResource>(returnResource);
        }

        public IEnumerable<IEnumerable<string>> GetSpendByPartExport(int supplierId)
        {
            return this.domainService.GetSpendByPartReport(supplierId).ConvertToCsvList();
        }

        public IResult<ReportReturnResource> GetSpendByPartReport(int supplierId)
        {
            var results = this.domainService.GetSpendByPartReport(supplierId);

            var returnResource = this.BuildResource(results);

            return new SuccessResult<ReportReturnResource>(returnResource);
        }

        private ReportReturnResource BuildResource(ResultsModel resultsModel)
        {
            return (ReportReturnResource)this.resultsModelResourceBuilder.Build(
                new List<ResultsModel> { resultsModel });
        }
    }
}
