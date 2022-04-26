namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;

    public interface IWhatsInInspectionReportFacadeService
    {
        IResult<WhatsInInspectionReportResource> GetReport(
            bool includePartsWithNoOrderNumber = false,
            bool showStockLocations = true,
            bool includeFailedStock = false,
            bool includeFinishedGoods = false,
            bool showBackOrdered = true,
            bool showOrders = true);

        IEnumerable<IEnumerable<string>> GetTopLevelExport(
            bool includePartsWithNoOrderNumber = false,
            bool includeFailedStock = false,
            bool includeFinishedGoods = true);
    }
}
