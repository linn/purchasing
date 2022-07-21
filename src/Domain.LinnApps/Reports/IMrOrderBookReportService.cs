namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Collections.Generic;

    using Linn.Common.Reporting.Models;

    public interface IMrOrderBookReportService
    {
        IEnumerable<ResultsModel> GetOrderBookReport(int supplierId);

        ResultsModel GetOrderBookExport(int supplierId);
    }
}
