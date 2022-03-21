namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;

    using Linn.Common.Reporting.Models;

    public interface IWhatsDueInReportService
    {
        ResultsModel GetReport(
            DateTime fromDate, DateTime toDate, string orderBy, string vendorManager, int? supplier);
    }
}
