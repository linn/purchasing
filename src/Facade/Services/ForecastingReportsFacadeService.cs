namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Purchasing.Domain.LinnApps.Reports;

    public class ForecastingReportsFacadeService : IForecastingReportsFacadeService
    {
        private readonly IForecastOrdersReportService domainService;

        public ForecastingReportsFacadeService(IForecastOrdersReportService domainService)
        {
            this.domainService = domainService;
        }

        public IEnumerable<IEnumerable<string>> GetWeeklyForecastExport(int supplierId)
        {
            return this.domainService.GetWeeklyExport(supplierId).ConvertToCsvList();
        }
    }
}
