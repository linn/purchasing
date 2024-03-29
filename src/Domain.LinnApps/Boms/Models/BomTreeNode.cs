﻿namespace Linn.Purchasing.Domain.LinnApps.Boms.Models
{
    using System;
    using System.Collections.Generic;

    public class BomTreeNode
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Qty { get; set; }

        public IEnumerable<BomTreeNode> Children { get; set; }

        public string Type { get; set; }

        public string ParentName { get; set; }

        public string ParentId { get; set; }

        public string Id { get; set; }

        public bool? HasChanged { get; set; }

        public bool? AssemblyHasChanges { get; set; }

        public string ChangeState { get; set; }

        public int AddChangeDocumentNumber { get; set; }

        public int? DeleteChangeDocumentNumber { get; set; }

        public bool IsReplaced { get; set; }

        public string ReplacementFor { get; set; }

        public string ReplacedBy { get; set; }

        public int? AddReplaceSeq { get; set; }

        public int? DeleteReplaceSeq { get; set; }

        public bool? ToDelete { get; set; }

        public string Requirement { get; set; }

        public string DrawingReference { get; set; }

        public string SafetyCritical { get; set; }

        public string PcasLine { get; set; }

        public int? AddChangeId { get; set; }

        public int? DeleteChangeId { get; set; }

        public DateTime? AddedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public decimal? Cost { get; set; }
    }
}
