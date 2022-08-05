namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using Linn.Common.Reporting.Models;

    public interface IDeliveryPerformanceReportService
    {
        ResultsModel GetDeliveryPerformanceSummary(int startPeriod, int endPeriod, int? supplierId, string vendorManager);
    }
}
