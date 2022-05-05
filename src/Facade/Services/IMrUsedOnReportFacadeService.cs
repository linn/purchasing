namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IMrUsedOnReportFacadeService
    {
        IResult<ReportReturnResource> GetReport(string partNumber);
    }
}
