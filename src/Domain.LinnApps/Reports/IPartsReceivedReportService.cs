namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using Linn.Common.Reporting.Models;

    public interface IPartsReceivedReportService
    {
        ResultsModel GetReport(
            string jobref,
            int? supplier,
            string fromDate,
            string toDate,
            string orderBy,
            bool includeNegativeValues = true);
    }
}
