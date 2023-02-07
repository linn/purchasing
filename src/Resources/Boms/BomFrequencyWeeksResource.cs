namespace Linn.Purchasing.Resources.Boms
{
    using Linn.Common.Resources;

    public class BomFrequencyWeeksResource : HypermediaResource
    {
        public int FreqWeeks { get; set; }

        public string PartNumber { get; set; }
    }
}
