namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class SupplierContactResourceBuilder : IBuilder<SupplierContact>
    {
        public SupplierContactResource Build(SupplierContact entity, IEnumerable<string> claims)
        {
            return new SupplierContactResource
                       {
                          SupplierId = entity.SupplierId,
                          DateInvalid = entity.DateInvalid?.ToString("o"),
                          PhoneNumber = entity.PhoneNumber,
                          DateCreated = entity.DateCreated?.ToString("o"),
                          ContactId = entity.ContactId,
                          MainInvoiceContact = entity.MainInvoiceContact,
                          MainOrderContact = entity.MainOrderContact,
                          JobTitle = entity.JobTitle,
                          PersonId = entity.PersonId,
                          Comments = entity.Comments,
                          ContactDescription = entity.ContactDescription,
                          EmailAddress = entity.EmailAddress,
                          MobileNumber = entity.MobileNumber
                       };
        }

        public string GetLocation(SupplierContact p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<SupplierContact>.Build(SupplierContact entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
