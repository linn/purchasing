﻿namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using Linn.Common.Reporting.Models;

    public interface ISpendsReportService
    {
        ResultsModel GetSpendBySupplierReport(string vendorManagerId);

        ResultsModel GetSpendBySupplierByDateRangeReport(
            string from,
            string to,
            string vendorManagerId,
            int? supplierId);

        ResultsModel GetSpendByPartReport(int supplierId);

        ResultsModel GetSpendByPartByDateReport(int supplierId, string fromDate, string toDate);
    }
}
