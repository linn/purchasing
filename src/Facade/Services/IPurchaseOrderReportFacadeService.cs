namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.IO;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public interface IPurchaseOrderReportFacadeService
    {
        IResult<ReportReturnResource> GetOrdersBySupplierReport(
            OrdersBySupplierSearchResource resource,
            IEnumerable<string> privileges);

        IEnumerable<IEnumerable<string>> GetOrdersBySupplierExport(
            OrdersBySupplierSearchResource resource,
            IEnumerable<string> privileges);

        IResult<ReportReturnResource> GetOrdersByPartReport(
            OrdersByPartSearchResource resource,
            IEnumerable<string> privileges);

        IEnumerable<IEnumerable<string>> GetOrdersByPartExport(
            OrdersByPartSearchResource resource,
            IEnumerable<string> privileges);

        IResult<ReportReturnResource> GetSuppliersWithUnacknowledgedOrdersReport(
            SuppliersWithUnacknowledgedOrdersRequestResource resource,
            IEnumerable<string> getPrivileges);

        IResult<ReportReturnResource> GetUnacknowledgedOrdersReport(
            UnacknowledgedOrdersRequestResource resource,
            IEnumerable<string> getPrivileges);

        IEnumerable<IEnumerable<string>> GetUnacknowledgedOrdersReportExport(
            UnacknowledgedOrdersRequestResource resource,
            IEnumerable<string> getPrivileges);
    }
}
