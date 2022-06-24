namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources.RequestResources;

    public interface IShortagesReportFacadeService
    {
        public IResult<IEnumerable<ReportReturnResource>> GetShortagesReport(ShortagesReportRequestResource options);

        public IResult<IEnumerable<ReportReturnResource>> GetShortagesPlannerReport(int planner);
    }
}
