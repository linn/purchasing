namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using Microsoft.VisualBasic;

    public interface IBomHistoryReportService
    {
        IEnumerable<BomHistoryViewEntry> GetReport(
            string bomName, DateTime from, DateTime to);

        IEnumerable<BomHistoryViewEntry> GetReportWithSubAssemblies(
            string bomName, DateTime from, DateTime to);
    }
}
