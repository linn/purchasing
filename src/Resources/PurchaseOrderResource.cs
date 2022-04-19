namespace Linn.Purchasing.Resources
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Resources;

    public class PurchaseOrderResource : HypermediaResource
    {
        public int OrderNumber { get; set; }

        public string CurrCode { get; set; }

        public DateTime DateOfOrder { get; set; }

        public string OrderMethod { get; set; }

        public string Cancelled { get; set; }

        public string Overbook { get; set; }

        public string DocumentType { get; set; }

        public int SupplierId { get; set; }
        //todo add supplier object, also containing phone number and email address for supplier contact for orders

        public decimal? OverbookQty { get; set; }
    }
}
