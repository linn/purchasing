namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources.RequestResources;

    public interface ISpendsReportFacadeService
    {
        IResult<IEnumerable<IEnumerable<string>>> GetSpendBySupplierExport(string vendorManagerId);

        IResult<ReportReturnResource> GetSpendBySupplierReport(string vendorManagerId);

        IResult<IEnumerable<IEnumerable<string>>> GetSpendBySupplierByDateRangeReportExport(SpendBySupplierByDateRangeReportRequestResource options);

        IResult<ReportReturnResource> GetSpendBySupplierByDateRangeReport(SpendBySupplierByDateRangeReportRequestResource options);

        IResult<IEnumerable<IEnumerable<string>>> GetSpendByPartExport(int supplierId);

        IResult<ReportReturnResource> GetSpendByPartReport(int supplierId);

        IResult<ReportReturnResource> GetSpendByPartByDateReport(SpendBySupplierByDateRangeReportRequestResource options);
    }
}
