namespace Linn.Purchasing.Domain.LinnApps
{
    using System;

    public class Contact
    {
        public string JobTitle { get; set; }

        public string PhoneNumber { get; set; }

        public string MobileNumber { get; set; }
        
        public string EmailAddress { get; set; }
        
        public string Comments { get; set; }
        
        public DateTime? DateInvalid { get; set; }
        
        public DateTime? DateCreated { get; set; }
        
        public int ContactId { get; set; }
        
        public Person Person { get; set; }

        public Organisation Organisation { get; set; }
    }
}
