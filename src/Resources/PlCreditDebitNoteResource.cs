﻿namespace Linn.Purchasing.Resources
{
    using System.Collections.Generic;

    using Linn.Common.Resources;

    public class PlCreditDebitNoteResource : HypermediaResource
    {
        public int NoteNumber { get; set; }

        public string NoteType { get; set; }

        public string PartNumber { get; set; }

        public int OrderQty { get; set; }

        public int? OriginalOrderNumber { get; set; }

        public int? ReturnsOrderNumber { get; set; }

        public decimal NetTotal { get; set; }

        public string Notes { get; set; }

        public string DateClosed { get; set; }

        public int? ClosedBy { get; set; }

        public string ReasonClosed { get; set; }

        public int? SupplierId { get; set; }

        public string SupplierName { get; set; }

        public IEnumerable<string> UserPrivileges { get; set; }

        public string DateCreated { get; set; }

        public bool? Close { get; set; }
    }
}
