namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.IO;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface ISpendsReportFacadeService
    {
        IEnumerable<IEnumerable<string>> GetSpendBySupplierExport(string vendorManagerId);

        IResult<ReportReturnResource> GetSpendBySupplierReport(string vendorManagerId);

        IEnumerable<IEnumerable<string>> GetSpendByPartExport(int supplierId);

        IResult<ReportReturnResource> GetSpendByPartReport(int supplierId);
    }
}
