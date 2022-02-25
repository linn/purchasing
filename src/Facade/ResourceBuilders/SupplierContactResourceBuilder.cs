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
                           IsMainInvoiceContact = entity.IsMainInvoiceContact,
                           IsMainOrderContact = entity.IsMainOrderContact,
                           LastName = entity.Person.LastName,
                           FirstName = entity.Person.FirstName,
                           Id = entity.ContactId,
                           EmailAddress = entity.EmailAddress,
                           MobileNumber = entity.MobileNumber,
                           PhoneNumber = entity.PhoneNumber,
                           PersonId = entity.Person.Id
                       };
        }

        public string GetLocation(SupplierContact p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<SupplierContact>.Build(SupplierContact entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
