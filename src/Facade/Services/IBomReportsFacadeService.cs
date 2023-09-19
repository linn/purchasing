namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.IO;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public interface IBomReportsFacadeService
    {
        IResult<ReportReturnResource> GetPartsOnBomReport(string bomName);

        IResult<ReportReturnResource> GetBomDifferencesReport(
            string bom1, string bom2, bool singleLevel, bool showDescriptions);

        IResult<IEnumerable<BomCostReportResource>> GetBomCostReport(
            string bomName,
            bool splitBySubAssembly,
            int levels,
            decimal labourHourlyRate);

        IResult<ReportReturnResource> GetBoardDifferenceReport(BomDifferenceReportRequestResource resource);

        IResult<ReportReturnResource> GetBoardComponentSummaryReport(BoardComponentSummaryReportRequestResource resource);

        Stream GetBomCostReportPdf(
            string bomName,
            bool splitBySubAssembly,
            int levels,
            decimal labourHourlyRate);
    }
}
