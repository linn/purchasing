namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IMrOrderBookReportFacadeService
    {
        IResult<ReportReturnResource> GetReport(int supplierId);

        IResult<ReportReturnResource> GetExportReport(int supplierId);
    }
}
