namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Resources.RequestResources;

    public class ShortagesReportFacadeService : IShortagesReportFacadeService
    {
        private readonly IShortagesReportService domainService;

        public ShortagesReportFacadeService(
            IShortagesReportService domainService,
            IBuilder<ResultsModel> resultsModelResourceBuilder)
        {
            this.domainService = domainService;
        }

        public IResult<IEnumerable<ResultsModel>> GetReport(ShortagesReportRequestResource options)
        {
            return new SuccessResult<IEnumerable<ResultsModel>>(
                this.domainService.GetReport(
                    options.PurchaseLevel,
                    options.Supplier,
                    options.VendorManager));
        }
    }
}
