namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public interface IWhatsInInspectionReportService
    {
        WhatsInInspectionReport GetReport(
            bool showGoodStockQty = false,
            bool includePartsWithNoOrderNumber = false,
            bool showStockLocations = true,
            bool includeFailedStock = false,
            bool includeFinishedGoods = true,
            bool showBackOrdered = true,
            bool showOrders = true);

        ResultsModel GetTopLevelReport(
            bool showGoodStockQty = false,
            bool includePartsWithNoOrderNumber = false,
            bool includeFailedStock = false,
            bool includeFinishedGoods = true);
    }
}
