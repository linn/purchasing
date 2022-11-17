namespace Linn.Purchasing.Resources.Boms
{
    using Linn.Common.Resources;

    public class CircuitBoardResource : HypermediaResource
    {
        public string BoardCode { get; set; }

        public string Description { get; set; }

        public int? ChangeId { get; set; }

        public string ChangeState { get; set; }

        public string SplitBom { get; set; }

        public string DefaultPcbNumber { get; set; }

        public string VariantOfBoardCode { get; set; }

        public string LoadDirectory { get; set; }

        public int? BoardsPerSheet { get; set; }

        public string CoreBoard { get; set; }

        public string ClusterBoard { get; set; }

        public string IdBoard { get; set; }
    }
}
