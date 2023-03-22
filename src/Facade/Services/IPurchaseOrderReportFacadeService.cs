namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public interface IPurchaseOrderReportFacadeService
    {
        IResult<ReportReturnResource> GetOrdersBySupplierReport(
            OrdersBySupplierSearchResource resource);

        IResult<ReportReturnResource> GetOrdersByPartReport(
            OrdersByPartSearchResource resource);

        IResult<ReportReturnResource> GetSuppliersWithUnacknowledgedOrdersReport(
            SuppliersWithUnacknowledgedOrdersRequestResource resource);

        IResult<ReportReturnResource> GetUnacknowledgedOrdersReport(
            UnacknowledgedOrdersRequestResource resource);

        IResult<ReportReturnResource> GetDeliveryPerformanceSummaryReport(
            DeliveryPerformanceRequestResource requestResource);

        IResult<ReportReturnResource> GetDeliveryPerformanceSupplierReport(
            DeliveryPerformanceRequestResource requestResource);

        IResult<ReportReturnResource> GetDeliveryPerformanceDetailReport(
            DeliveryPerformanceRequestResource requestResource);
    }
}
