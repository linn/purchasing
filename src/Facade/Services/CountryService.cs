namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    public class CountryService : FacadeResourceService<Country, string, CountryResource, CountryResource>
    {
        public CountryService(IRepository<Country, string> repository, ITransactionManager transactionManager, IBuilder<Country> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Country CreateFromResource(CountryResource resource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(Country entity, CountryResource updateResource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Country, bool>> SearchExpression(string searchTerm)
        {
            return c => c.Name.Contains(searchTerm.ToUpper()) || c.CountryCode.Equals(searchTerm.ToUpper());
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            Country entity,
            CountryResource resource,
            CountryResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(Country entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
