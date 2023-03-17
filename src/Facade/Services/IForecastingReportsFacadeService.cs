namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;

    public interface IForecastingReportsFacadeService
    {
        IResult<IEnumerable<IEnumerable<string>>> GetMonthlyForecastForSupplier(int supplierId);
    }
}
