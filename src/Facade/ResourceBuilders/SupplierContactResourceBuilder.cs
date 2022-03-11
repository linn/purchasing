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
                           ContactName = $"{entity.Person?.FirstName} {entity.Person?.LastName}",
                           Email = entity.Email,
                           PhoneNumber = entity.PhoneNumber
                       };
        }

        public string GetLocation(SupplierContact s)
        {
            throw new NotImplementedException();
        }

        object IBuilder<SupplierContact>.Build(SupplierContact entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }
    }
}
