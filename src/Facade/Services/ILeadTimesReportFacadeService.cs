namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources.RequestResources;

    public interface ILeadTimesReportFacadeService
    {
        public IResult<ReportReturnResource> GetLeadTimesWithSupplierReport(LeadTimesReportRequestResource options);
    }
}
