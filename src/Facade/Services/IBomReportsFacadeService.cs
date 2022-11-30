﻿namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources;

    public interface IBomReportsFacadeService
    {
        IResult<ReportReturnResource> GetPartsOnBomReport(string bomName);

        IEnumerable<IEnumerable<string>> GetPartsOnBomExport(string bomName);

        IResult<IEnumerable<BomCostReportResource>> GetBomCostReport(
            string bomName,
            bool splitBySubAssembly,
            int levels,
            decimal labourHourlyRate);
    }
}