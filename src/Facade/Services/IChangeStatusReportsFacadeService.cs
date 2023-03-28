namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IChangeStatusReportsFacadeService
    {
        IResult<ReportReturnResource> GetChangeStatusReport(int months);

        IResult<ReportReturnResource> GetAcceptedChangesReport(int months);

        IResult<ReportReturnResource> GetProposedChangesReport(int months);

        IResult<ReportReturnResource> GetTotalOutstandingChangesReport(int months);

        IResult<ReportReturnResource> GetCurrentPhaseInWeeksReport(int months);
    }
}
