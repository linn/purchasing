namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IMrOrderBookReportFacadeService
    {
        IResult<ReportReturnResource> GetReport(int supplierId);

        IEnumerable<IEnumerable<string>> GetExport(int supplierId);
    }
}
