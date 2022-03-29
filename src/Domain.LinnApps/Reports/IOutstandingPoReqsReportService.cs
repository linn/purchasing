namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using Linn.Common.Reporting.Models;

    public interface IOutstandingPoReqsReportService
    {
        ResultsModel GetReport(string state);
    }
}
