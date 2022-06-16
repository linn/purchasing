namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;

    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public class ReportReturnResourceBuilder : IReportReturnResourceBuilder
    {
        public ReportReturnResource Build(IEnumerable<ResultsModel> models)
        {
            var returnResource = new ReportReturnResource();
            foreach (var model in models)
            {
                returnResource.ReportResults.Add(model.ConvertFinalModelToResource());
            }
            
            return returnResource;
        }
    }
}
