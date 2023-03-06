namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using Linn.Common.Reporting.Models;

    public interface IChangeStatusReportService
    {
        ResultsModel GetChangeStatusReport(int months);

        ResultsModel GetAcceptedChangesReport(int months);

        ResultsModel GetProposedChangesReport(int months);

        ResultsModel GetTotalOutstandingChangesReport(int months);
    }
}
