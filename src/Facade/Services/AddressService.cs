namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    public class AddressService : FacadeFilterResourceService<Address, int, AddressResource, AddressResource, AddressResource>
    {
        public AddressService(IRepository<Address, int> repository, ITransactionManager transactionManager, IBuilder<Address> builder)
            : base(repository, transactionManager, builder)
        {
        }

        protected override Address CreateFromResource(AddressResource resource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(Address entity, AddressResource updateResource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Address, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            Address entity,
            AddressResource resource,
            AddressResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(Address entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Address, bool>> FilterExpression(AddressResource searchResource)
        {
            return x => x.Addressee.Contains(searchResource.Addressee.ToUpper());
        }
    }
}
