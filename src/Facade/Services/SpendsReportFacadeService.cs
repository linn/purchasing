﻿namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.IO;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Serialization;
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

        public Stream GetSpendBySupplierExport(string vm, IEnumerable<string> privileges)
        {
            var results = this.domainService.GetSpendBySupplierReport(vm);

            var returnResource = results.ConvertToCsvList();

            var stream = new MemoryStream();
            var csvStreamWriter = new CsvStreamWriter(stream);
            csvStreamWriter.WriteModel(returnResource);

            return stream;
        }

        public IResult<ReportReturnResource> GetSpendBySupplierReport(string vm, IEnumerable<string> privileges)
        {
            var results = this.domainService.GetSpendBySupplierReport(vm);

            var returnResource = this.BuildResource(results, privileges);

            return new SuccessResult<ReportReturnResource>(returnResource);
        }

        private ReportReturnResource BuildResource(ResultsModel resultsModel, IEnumerable<string> privileges)
        {
            return (ReportReturnResource)this.resultsModelResourceBuilder.Build(resultsModel, privileges);
        }
    }
}
