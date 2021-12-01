namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Resources;

    public interface IPurchaseOrderReportFacadeService
    {
        IResult<ResultsModel> GetOrdersBySupplierReport(OrdersBySupplierSearchResource resource);
    }
}
