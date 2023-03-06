namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public interface IChangeStatusReportsFacadeService
    {
        IResult<ReportReturnResource> GetChangeStatusReport(ChangeStatusReportRequestResource resource);

        IResult<ReportReturnResource> GetAcceptedChangesReport(ChangeStatusReportRequestResource resource);

        IResult<ReportReturnResource> GetProposedChangesReport(ChangeStatusReportRequestResource resource);

        IResult<ReportReturnResource> GetTotalOutstandingChangesReport(ChangeStatusReportRequestResource resource);

    }
}
