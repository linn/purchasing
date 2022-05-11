namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Resources.RequestResources;

    public interface IShortagesReportFacadeService
    {
        public IResult<IEnumerable<ResultsModel>> GetReport(ShortagesReportRequestResource options);
    }
}

