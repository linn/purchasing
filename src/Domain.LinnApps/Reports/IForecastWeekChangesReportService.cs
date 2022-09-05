namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using Linn.Common.Reporting.Models;

    public interface IForecastWeekChangesReportService
    {
        ResultsModel GetReport();
    }
}
