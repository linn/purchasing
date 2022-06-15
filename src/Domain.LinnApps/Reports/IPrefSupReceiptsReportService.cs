namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;

    using Linn.Common.Reporting.Models;

    public interface IPrefSupReceiptsReportService
    {
        ResultsModel GetReport(DateTime fromDate, DateTime toDate, bool justGBP = false);
    }
}
