namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    public class MrReportOptionsResourceBuilder : IBuilder<MrReportOptions>
    {
        public object Build(MrReportOptions model, IEnumerable<string> claims)
        {
            return new MrReportOptionsResource
                       {
                           PartSelectorOptions =
                               model.PartSelectorOptions.Select(this.BuildOptionResource),
                           StockLevelOptions = model.StockLevelOptions.Select(this.BuildOptionResource),
                           OrderByOptions = model.OrderByOptions.Select(this.BuildOptionResource),
                           PartOptions = model.PartOptions.Select(this.BuildOptionResource),
                           AvailableJobRefs = model.AvailableJobRefs.Select(a => new MrpRunLogResource
                               {
                                   MrRunLogId = a.MrRunLogId,
                                   JobRef = a.JobRef,
                                   RunDate = a.RunDate.ToString("o")
                               })
                       };
        }

        public string GetLocation(MrReportOptions model)
        {
            throw new System.NotImplementedException();
        }

        private ReportOptionResource BuildOptionResource(ReportOption option)
        {
            return new ReportOptionResource
                       {
                           DisplayText = option.DisplayText,
                           Option = option.Option,
                           DisplaySequence = option.DisplaySequence,
                           DataTag = option.DataTag,
                           DefaultOption = option.DefaultOption
                       };
        }
    }
}
