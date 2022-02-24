namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class SupplierContactResourceBuilder : IBuilder<SupplierContact>
    {
        private readonly IBuilder<Contact> contactResourceBuilder;

        public SupplierContactResourceBuilder(IBuilder<Contact> contactResourceBuilder)
        {
            this.contactResourceBuilder = contactResourceBuilder;
        }

        public SupplierContactResource Build(SupplierContact entity, IEnumerable<string> claims)
        {
            return new SupplierContactResource
            {
                SupplierId = entity.SupplierId,
                Contact = (ContactResource)this.contactResourceBuilder.Build(entity.Contact, null),
                IsMainInvoiceContact = entity.IsMainInvoiceContact,
                IsMainOrderContact = entity.IsMainOrderContact
            };
        }

        public string GetLocation(SupplierContact p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<SupplierContact>.Build(SupplierContact entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
