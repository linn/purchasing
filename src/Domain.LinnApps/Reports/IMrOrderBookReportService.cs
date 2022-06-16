namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using Linn.Common.Reporting.Models;

    public interface IMrOrderBookReportService
    {
        ResultsModel GetOrderBookReport(int supplierId);
    }
}
