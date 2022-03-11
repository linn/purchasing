namespace Linn.Purchasing.Domain.LinnApps
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    public class Employee
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public IEnumerable<SigningLimit> SigningLimits { get; set; }

        public PhoneListEntry PhoneListEntry { get; set; }
    }
}
