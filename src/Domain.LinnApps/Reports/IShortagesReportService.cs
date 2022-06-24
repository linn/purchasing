namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Collections.Generic;

    using Linn.Common.Reporting.Models;

    public interface IShortagesReportService
    {
        IEnumerable<ResultsModel> GetShortagesReport(int purchaseLevel, string vendorManager);

        IEnumerable<ResultsModel> GetShortagesPlannerReport(int planner);
    }
}
