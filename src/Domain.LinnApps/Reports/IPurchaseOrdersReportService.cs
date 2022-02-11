namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;

    using Linn.Common.Reporting.Models;

    public interface IPurchaseOrdersReportService
    {
        ResultsModel GetOrdersByPartReport(DateTime from, DateTime to, string partNumber, bool includeCancelled);

        ResultsModel GetOrdersBySupplierReport(
            DateTime from,
            DateTime to,
            int supplierId,
            bool includeReturns,
            bool outstandingOnly,
            bool includeCancelled,
            string includeCredits,
            string stockControlled);

        ResultsModel GetSuppliersWithUnacknowledgedOrders(int? planner, string vendorManager);

        ResultsModel GetUnacknowledgedOrders(DateTime startDate, DateTime endDate, int supplierId, int? organisationId);
    }
}
