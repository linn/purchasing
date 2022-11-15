﻿namespace Linn.Purchasing.Domain.LinnApps.Boms.Models
{
    using System.Collections.Generic;

    public class BomTreeNode
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Qty { get; set; }

        public IEnumerable<BomTreeNode> Children { get; set; }
    }
}