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
    }
}
