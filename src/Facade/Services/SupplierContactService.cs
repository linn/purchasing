namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class SupplierContactService 
        : FacadeResourceService<SupplierContact, int, SupplierContactResource, SupplierContactResource>
    {
        public SupplierContactService(
            IRepository<SupplierContact, int> repository, 
            ITransactionManager transactionManager, 
            IBuilder<SupplierContact> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override SupplierContact CreateFromResource(
            SupplierContactResource resource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(
            SupplierContact entity, 
            SupplierContactResource updateResource, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<SupplierContact, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            SupplierContact entity,
            SupplierContactResource resource,
            SupplierContactResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            SupplierContact entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}