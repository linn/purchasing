namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    public interface IForecastingReportsFacadeService
    {
        IEnumerable<IEnumerable<string>> GetWeeklyForecastExport(int supplierId);
    }
}
