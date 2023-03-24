namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Purchasing.Domain.LinnApps.Reports;

    public class ForecastingReportsFacadeService : IForecastingReportsFacadeService
    {
        private readonly IForecastOrdersReportService domainService;

        public ForecastingReportsFacadeService(IForecastOrdersReportService domainService)
        {
            this.domainService = domainService;
        }

        public IResult<IEnumerable<IEnumerable<string>>> GetMonthlyForecastForSupplier(int supplierId)
        {
            return new SuccessResult<IEnumerable<IEnumerable<string>>>(this.domainService.GetMonthlyExport(supplierId));
        }
    }
}
