namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using Linn.Common.Reporting.Models;

    public interface ISpendsReportService
    {
        ResultsModel GetSpendBySupplierReport(string vendorManagerId);

        ResultsModel GetSpendByPartReport(int supplierId);

    }
}
