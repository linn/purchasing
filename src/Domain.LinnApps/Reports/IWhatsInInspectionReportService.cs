namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public interface IWhatsInInspectionReportService
    {
        IEnumerable<WhatsInInspectionReportModel> GetReport(
            bool includePartsWithNoOrderNumber = false,
            bool showStockLocations = true,
            bool includeFailedStock = false,
            bool includeFinishedGoods = true,
            bool showBackOrdered = true);
    }
}
