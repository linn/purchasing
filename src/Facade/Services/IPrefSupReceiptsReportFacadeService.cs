namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IPrefSupReceiptsReportFacadeService
    {
        IResult<ReportReturnResource> GetReport(string fromDate, string toDate);
    }
}
