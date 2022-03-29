namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IOutstandingPoReqsReportFacadeService
    {
        IResult<ReportReturnResource> GetReport(string state);
    }
}
