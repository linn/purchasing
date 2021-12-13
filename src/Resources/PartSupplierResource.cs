namespace Linn.Purchasing.Resources
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Resources;

    public class PartSupplierResource : HypermediaResource
    {
        public string PartNumber { get; set; }

        public string PartDescription { get; set; }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public int? PackingGroup { get; set; }

        public string PackingGroupDescription { get; set; }

        public int? CreatedBy { get; set; }

        public string CreatedByName { get; set; }

        public string OrderMethodName { get; set; }

        public string OrderMethodDescription { get; set; }

        public int? TariffId { get; set; }

        public string TariffCode { get; set; }

        public string TariffDescription { get; set; }

        public string ManufacturerCode { get; set; }

        public string ManufacturerName { get; set; }

        public int? AddressId { get; set; }

        public string FullAddress { get; set; }

        public string Designation { get; set; }

        public string Stream { get; set; }

        public string CurrencyCode { get; set; }

        public string UnitOfMeasure { get; set; }

        public IEnumerable<string> Privileges { get; set; }
    }
}
