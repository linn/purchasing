namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Domain.LinnApps.Reports;

    public class SpendsReportFacadeService : ISpendsReportFacadeService
    {
        private readonly ISpendsReportService domainService;

        private readonly IBuilder<ResultsModel> resultsModelResourceBuilder;

        public SpendsReportFacadeService(
            ISpendsReportService domainService,
            IBuilder<ResultsModel> resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IEnumerable<IEnumerable<string>> GetSpendBySupplierExport(string vendorManagerId, IEnumerable<string> privileges)
        {
            return this.domainService.GetSpendBySupplierReport(vendorManagerId).ConvertToCsvList();
        }

        public IResult<ReportReturnResource> GetSpendBySupplierReport(string vendorManagerId, IEnumerable<string> privileges)
        {
            var results = this.domainService.GetSpendBySupplierReport(vendorManagerId);

            var returnResource = this.BuildResource(results, privileges);

            return new SuccessResult<ReportReturnResource>(returnResource);
        }

        public IEnumerable<IEnumerable<string>> GetSpendByPartExport(int supplierId, IEnumerable<string> privileges)
        {
            return this.domainService.GetSpendByPartReport(supplierId).ConvertToCsvList();
        }

        public IResult<ReportReturnResource> GetSpendByPartReport(int supplierId, IEnumerable<string> privileges)
        {
            var results = this.domainService.GetSpendByPartReport(supplierId);

            var returnResource = this.BuildResource(results, privileges);

            return new SuccessResult<ReportReturnResource>(returnResource);
        }

        private ReportReturnResource BuildResource(ResultsModel resultsModel, IEnumerable<string> privileges)
        {
            return (ReportReturnResource)this.resultsModelResourceBuilder.Build(resultsModel, privileges);
        }
    }
}
