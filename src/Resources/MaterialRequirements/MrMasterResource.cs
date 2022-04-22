namespace Linn.Purchasing.Resources.MaterialRequirements
{
    using Linn.Common.Resources;

    public class MrMasterResource : HypermediaResource
    {
        public string JobRef { get; set; }

        public string RunDate { get; set; }
    }
}
