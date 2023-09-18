namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IBomReportsService
    {
        ResultsModel GetPartsOnBomReport(string bomName);

        IEnumerable<BomCostReport> GetBomCostReport(
            string bomName,
            bool splitBySubAssembly,
            int levels,
            decimal labourHourlyRate);

        ResultsModel GetBoardDifferenceReport(
            string boardCode1,
            string revisionCode1,
            string boardCode2,
            string revisionCode2,
            bool liveOnly);

        ResultsModel GetBomDifferencesReport(string bom1, string bom2, bool singleLevel = true);

        ResultsModel GetBoardComponentSummaryReport(
            string boardCode,
            string revisionCode);
    }
}
