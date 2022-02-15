namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.IO;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface ISpendsReportFacadeService
    {
        Stream GetSpendBySupplierExport(string vendorManagerId, IEnumerable<string> privileges);

        IResult<ReportReturnResource> GetSpendBySupplierReport(string vendorManagerId, IEnumerable<string> privileges);

        Stream GetSpendByPartExport(int supplierId, IEnumerable<string> privileges);

        IResult<ReportReturnResource> GetSpendByPartReport(int supplierId, IEnumerable<string> privileges);
    }
}
