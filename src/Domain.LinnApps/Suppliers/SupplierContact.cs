namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    using System;

    public class SupplierContact
    {
        public string ContactDescription { get; set; }
        
        public string MainOrderContact { get; set; }
        
        public string MainInvoiceContact { get; set; }
        
        public string MobileNumber { get; set; }
        
        public string EmailAddress { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string Comments { get; set; }
        
        public string JobTitle { get; set; }
        
        public DateTime? DateInvalid { get; set; }
        
        public DateTime? DateCreated { get; set; }
        
        public int ContactId { get; set; }
        
        public int SupplierId { get; set; }
        
        public int PersonId { get; set; }
    }
}
