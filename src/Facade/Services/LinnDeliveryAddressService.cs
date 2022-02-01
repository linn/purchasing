namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class LinnDeliveryAddressService 
        : FacadeResourceService<LinnDeliveryAddress, int, LinnDeliveryAddressResource, LinnDeliveryAddressResource>
    {
        public LinnDeliveryAddressService(IRepository<LinnDeliveryAddress, int> repository, ITransactionManager transactionManager, IBuilder<LinnDeliveryAddress> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override LinnDeliveryAddress CreateFromResource(LinnDeliveryAddressResource resource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            LinnDeliveryAddress entity,
            LinnDeliveryAddressResource resource,
            LinnDeliveryAddressResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(LinnDeliveryAddress entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(LinnDeliveryAddress entity, LinnDeliveryAddressResource updateResource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<LinnDeliveryAddress, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
