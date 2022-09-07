namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Collections.Generic;

    using Linn.Common.Reporting.Models;

    public interface IForecastOrdersReportService
    {
        IEnumerable<IEnumerable<string>> GetMonthlyExport(int supplierId);
    }
}
