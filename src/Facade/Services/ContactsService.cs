namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    public class ContactsService 
        : FacadeResourceService<Contact, int, ContactResource, ContactResource>
    {
        public ContactsService(IRepository<Contact, int> repository, ITransactionManager transactionManager, IBuilder<Contact> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Contact CreateFromResource(ContactResource resource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(Contact entity, ContactResource updateResource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Contact, bool>> SearchExpression(string searchTerm)
        {
            if (int.TryParse(searchTerm, out var key))
            {
                return x => x.ContactId == key;
            }

            return x => x.Person.FirstName.ToUpper().Contains(searchTerm.ToUpper())
                        || x.Person.LastName.ToUpper().Contains(searchTerm.ToUpper());
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            Contact entity,
            ContactResource resource,
            ContactResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(Contact entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
