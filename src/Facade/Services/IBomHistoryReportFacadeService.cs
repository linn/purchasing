namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;

    public interface IBomHistoryReportFacadeService
    {
        IResult<IEnumerable<BomHistoryReportLineResource>> GetReport(
            string bomName, string from, string to, bool includeSubAssemblies);
    }
}
