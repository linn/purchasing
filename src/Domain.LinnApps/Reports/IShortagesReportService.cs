namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Collections.Generic;

    using Linn.Common.Reporting.Models;

    public interface IShortagesReportService
    {
        IEnumerable<ResultsModel> GetReport(int purchaseLevel, string vendorManager);
    }
}
