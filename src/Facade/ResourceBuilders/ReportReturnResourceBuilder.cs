namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public class ReportReturnResourceBuilder : IBuilder<IEnumerable<ResultsModel>>
    {
        public ReportReturnResource Build(IEnumerable<ResultsModel> models, IEnumerable<string> claims)
        {
            var returnResource = new ReportReturnResource();
            foreach (var model in models)
            {
                returnResource.ReportResults.Add(model.ConvertFinalModelToResource());
            }
            
            return returnResource;
        }

        public string GetLocation(IEnumerable<ResultsModel> models)
        {
            throw new NotImplementedException();
        }

        object IBuilder<IEnumerable<ResultsModel>>.Build(IEnumerable<ResultsModel> models, IEnumerable<string> claims)
        {
            return this.Build(models, claims);
        }
    }
}
