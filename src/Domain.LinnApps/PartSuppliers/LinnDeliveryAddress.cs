namespace Linn.Purchasing.Domain.LinnApps.PartSuppliers
{
    using System;

    public class LinnDeliveryAddress
    {
        public FullAddress FullAddress { get; set; }

        public int AddressId { get; set; }

        public string Description { get; set; }

        public string IsMainDeliveryAddress { get; set; }

        public DateTime? DateObsolete { get; set; }
    }
}
