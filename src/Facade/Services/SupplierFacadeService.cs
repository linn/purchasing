namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class SupplierFacadeService : FacadeResourceService<Supplier, int, SupplierResource, SupplierResource>
    {
        public SupplierFacadeService(IRepository<Supplier, int> repository, ITransactionManager transactionManager, IBuilder<Supplier> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Supplier CreateFromResource(SupplierResource resource)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(Supplier entity, SupplierResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Supplier, bool>> SearchExpression(string searchTerm)
        {
            return s => s.Name.Contains(searchTerm.ToUpper());
        }
    }
}
