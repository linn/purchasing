namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using Linn.Common.Reporting.Models;

    public interface IMrUsedOnReportService
    {
        ResultsModel GetUsedOn(string partNumber);
    }
}
