namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;

    public interface IWhatsInInspectionReportFacadeService
    {
        IResult<IEnumerable<WhatsInInspectionReportResource>> GetReport(
            bool includePartsWithNoOrderNumber = false,
            bool showStockLocations = true,
            bool includeFailedStock = false,
            bool excludeFinishedGoods = false,
            bool showBackOrdered = true);
    }
}
