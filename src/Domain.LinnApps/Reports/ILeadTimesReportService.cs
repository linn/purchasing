namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using Linn.Common.Reporting.Models;

    public interface ILeadTimesReportService
    {
        ResultsModel GetLeadTimesBySupplier(int supplier);
    }
}
