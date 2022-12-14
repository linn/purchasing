namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    public class CircuitBoard
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

        public IList<BoardLayout> Layouts { get; set; }

        public IList<BoardComponent> Components { get; set; }
    }
}
