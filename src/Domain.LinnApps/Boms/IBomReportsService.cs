namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using Linn.Common.Reporting.Models;

    public interface IBomReportsService
    {
        ResultsModel GetPartsOnBomReport(string bomName);
    }
}

