namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;

    public interface IWhatsInInspectionReportFacadeService
    {
        IResult<WhatsInInspectionReportResource> GetReport(
            bool includePartsWithNoOrderNumber = false,
            bool showStockLocations = true,
            bool includeFailedStock = false,
            bool includeFinishedGoods = false,
            bool showBackOrdered = true);
    }
}
