namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;

    using Linn.Common.Reporting.Models;

    public interface IPurchaseOrdersReportService
    {
        ResultsModel GetOrdersByPartReport(DateTime from, DateTime to, string partNumber);

        ResultsModel GetOrdersBySupplierReport(DateTime from, DateTime to, int supplierId);
    }
}
