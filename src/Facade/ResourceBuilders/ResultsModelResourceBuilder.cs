namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Resources;

    public class ResultsModelResourceBuilder : IBuilder<ResultsModel>
    {
        public ReportReturnResource Build(ResultsModel resultsModel, IEnumerable<string> claims)
        {
            var returnResource = new ReportReturnResource();
            returnResource.ReportResults.Add(resultsModel.ConvertFinalModelToResource());
            return returnResource;
        }

        public string GetLocation(ResultsModel p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<ResultsModel>.Build(ResultsModel resultsModel, IEnumerable<string> claims)
        {
            return this.Build(resultsModel, claims);
        }

        private IEnumerable<LinkResource> BuildLinks(ResultsModel resultsModel, IEnumerable<string> claims)
        {
            yield return new LinkResource { Rel = "self", Href = this.GetLocation(resultsModel) };
        }
    }
}
