namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public interface IWhatsInInspectionReportService
    {
        WhatsInInspectionReportModel GetReport(
            bool includePartsWithNoOrderNumber = false,
            bool showStockLocations = true,
            bool includeFailedStock = false,
            bool excludeFinishedGoods = false,
            bool showBackOrdered = true);
    }
}
