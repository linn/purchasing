namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public interface IBomHistoryReportService
    {
        IEnumerable<BomHistoryReportLine> GetReport(
            string bomName, DateTime from, DateTime to);

        IEnumerable<BomHistoryReportLine> GetReportWithSubAssemblies(
            string bomName, DateTime from, DateTime to);
    }
}
