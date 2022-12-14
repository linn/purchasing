﻿namespace Linn.Purchasing.Domain.LinnApps.Boms.Models
{
    using System.Collections.Generic;

    public class BomTreeNode
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Qty { get; set; }

        public IEnumerable<BomTreeNode> Children { get; set; }

        public string Type { get; set; }

        public string ParentName { get; set; }

        public string Id { get; set; }

        public bool? HasChanged { get; set; }

        public string ChangeState { get; set; }

        public int AddChangeDocumentNumber { get; set; }

        public bool IsReplaced { get; set; }
    }
}
