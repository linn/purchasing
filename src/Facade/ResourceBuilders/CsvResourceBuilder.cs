//namespace Linn.Purchasing.Facade.ResourceBuilders
//{
//    using System;
//    using System.Collections.Generic;

//    using Linn.Common.Facade;
//    using Linn.Common.Reporting.Models;
//    using Linn.Common.Reporting.Resources.Extensions;
//    using Linn.Common.Reporting.Resources.ReportResultResources;
//    using Linn.Common.Resources;

//    public class CsvResourceBuilder : IBuilder<IEnumerable<IEnumerable<string>>>
//    {
//        public ReportReturnResource Build(IEnumerable<IEnumerable<string>> model, IEnumerable<string> claims)
//        {
//            var returnResource = new ReportReturnResource();
//            returnResource.ReportResults.Add(model.ConvertFinalModelToResource());
//            return returnResource;
//        }

//        public string GetLocation(IEnumerable<IEnumerable<string>> model)
//        {
//            throw new NotImplementedException();
//        }


//        object IBuilder<IEnumerable<IEnumerable<string>>>.Build(IEnumerable<IEnumerable<string>> model, IEnumerable<string> claims)
//        {
//            return this.Build(model, claims);
//        }

//        private IEnumerable<LinkResource> BuildLinks(ResultsModel resultsModel, IEnumerable<string> claims)
//        {
//            yield return new LinkResource { Rel = "self", Href = this.GetLocation(resultsModel) };
//        }
//    }
//}
