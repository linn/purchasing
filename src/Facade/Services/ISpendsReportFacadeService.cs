namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources.RequestResources;

    public interface ISpendsReportFacadeService
    {
        IResult<ReportReturnResource> GetSpendBySupplierReport(string vendorManagerId);

        IResult<ReportReturnResource> GetSpendBySupplierByDateRangeReport(SpendBySupplierByDateRangeReportRequestResource options);

        IResult<ReportReturnResource> GetSpendByPartReport(int supplierId);

        IResult<ReportReturnResource> GetSpendByPartByDateReport(SpendBySupplierByDateRangeReportRequestResource options);
    }
}
