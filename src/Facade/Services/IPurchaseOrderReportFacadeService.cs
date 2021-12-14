namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources;

    public interface IPurchaseOrderReportFacadeService
    {
        IResult<ReportReturnResource> GetOrdersBySupplierReport(
            OrdersBySupplierSearchResource resource,
            IEnumerable<string> privileges);
    }
}
