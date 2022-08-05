namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public interface IPurchaseOrderReportFacadeService
    {
        IResult<ReportReturnResource> GetOrdersBySupplierReport(
            OrdersBySupplierSearchResource resource);

        IEnumerable<IEnumerable<string>> GetOrdersBySupplierExport(
            OrdersBySupplierSearchResource resource);

        IResult<ReportReturnResource> GetOrdersByPartReport(
            OrdersByPartSearchResource resource);

        IEnumerable<IEnumerable<string>> GetOrdersByPartExport(
            OrdersByPartSearchResource resource);

        IResult<ReportReturnResource> GetSuppliersWithUnacknowledgedOrdersReport(
            SuppliersWithUnacknowledgedOrdersRequestResource resource);

        IResult<ReportReturnResource> GetUnacknowledgedOrdersReport(
            UnacknowledgedOrdersRequestResource resource);

        IEnumerable<IEnumerable<string>> GetUnacknowledgedOrdersReportExport(
            UnacknowledgedOrdersRequestResource resource);

        IResult<ReportReturnResource> GetDeliveryPerformanceSummaryReport(
            DeliveryPerformanceRequestResource requestResource);
    }
}
