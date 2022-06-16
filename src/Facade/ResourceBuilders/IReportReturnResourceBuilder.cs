namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;

    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IReportReturnResourceBuilder
    {
        ReportReturnResource Build(IEnumerable<ResultsModel> models);
    }
}
