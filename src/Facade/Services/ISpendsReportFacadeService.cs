namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.IO;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface ISpendsReportFacadeService
    {
        Stream GetSpendBySupplierExport(IEnumerable<string> privileges);

        IResult<ReportReturnResource> GetSpendBySupplierReport(IEnumerable<string> privileges);
    }
}
