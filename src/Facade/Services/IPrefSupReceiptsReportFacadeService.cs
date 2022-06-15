namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IPrefSupReceiptsReportFacadeService
    {
        IResult<ReportReturnResource> GetReport(string fromDate, string toDate);

        IEnumerable<IEnumerable<string>> GetExport(string fromDate, string toDate);
    }
}