namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    public class AddressService : FacadeFilterResourceService<Address, int, AddressResource, AddressResource, AddressResource>
    {
        private readonly IRepository<Country, string> countryRepository;

        private readonly IDatabaseService databaseService;

        public AddressService(
            IRepository<Address, int> repository, 
            ITransactionManager transactionManager, 
            IBuilder<Address> builder,
            IRepository<Country, string> countryRepository,
            IDatabaseService databaseService)
            : base(repository, transactionManager, builder)
        {
            this.countryRepository = countryRepository;
            this.databaseService = databaseService;
        }

        protected override Address CreateFromResource(AddressResource resource, IEnumerable<string> privileges = null)
        {
            return new Address
                       {
                           AddressId = this.databaseService.GetNextVal("ADDR_SEQ"),
                           Country = this.countryRepository.FindById(resource.CountryCode),
                           Line1 = resource.Line1,
                           Line2 = resource.Line2,
                           Line3 = resource.Line3,
                           Line4 = resource.Line4,
                           PostCode = resource.PostCode,
                           Addressee = resource.Addressee,
                           Addressee2 = resource.Addressee2
                       };
        }

        protected override void UpdateFromResource(Address entity, AddressResource updateResource, IEnumerable<string> privileges = null)
        {
            entity.Country = this.countryRepository.FindById(updateResource.CountryCode);
            entity.Line1 = updateResource.Line1;
            entity.Line2 = updateResource.Line2;
            entity.Line3 = updateResource.Line3;
            entity.Line4 = updateResource.Line4;
            entity.PostCode = updateResource.PostCode;
            entity.Addressee = updateResource.Addressee;
            entity.Addressee2 = updateResource.Addressee2;
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
            if (searchResource.AddressId.HasValue)
            {
                return x => x.AddressId == searchResource.AddressId.Value && !x.DateInvalid.HasValue;
            }
            
            return x => (x.Addressee.ToUpper().Contains(searchResource.Addressee.ToUpper()) 
                        || x.PostCode.ToUpper() == searchResource.PostCode.ToUpper())
                        && !x.DateInvalid.HasValue;
        }

        protected override Expression<Func<Address, bool>> FindExpression(AddressResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
