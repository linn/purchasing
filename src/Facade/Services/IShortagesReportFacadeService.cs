namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources.RequestResources;

    public interface IShortagesReportFacadeService
    {
        public IResult<ReportReturnResource> GetShortagesReport(ShortagesReportRequestResource options);

        public IResult<ReportReturnResource> GetShortagesPlannerReport(int planner);
    }
}
