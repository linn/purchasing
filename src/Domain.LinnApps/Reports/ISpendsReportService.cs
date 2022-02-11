namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;

    using Linn.Common.Reporting.Models;

    public interface ISpendsReportService
    {
        ResultsModel GetSpendBySupplierReport(string vm);
    }
}
