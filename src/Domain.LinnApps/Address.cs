namespace Linn.Purchasing.Domain.LinnApps
{
    using System;

    public class Address
    {
        public int AddressId { get; set; }

        public string Addressee { get; set; }

        public string Addressee2 { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Line3 { get; set; }

        public string Line4 { get; set; }

        public string PostCode { get; set; }

        public Country Country { get; set; }

        public FullAddress FullAddress { get; set; }
        
        public DateTime? DateInvalid { get; set; }
    }
}
