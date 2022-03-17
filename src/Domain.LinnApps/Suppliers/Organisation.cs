namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    using System;

    public class Organisation
    {
        public int OrgId { get; set; }

        public int AddressId { get; set; }

        public string Title { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public string WebAddress { get; set; }

        public string EmailAddress { get; set; }
    }
}
