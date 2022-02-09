namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Resources;

    public class PartCategoriesService : FacadeResourceService<PartCategory, string, PartCategoryResource, PartCategoryResource>
    {
        public PartCategoriesService(
            IRepository<PartCategory, string> repository, 
            ITransactionManager transactionManager, 
            IBuilder<PartCategory> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override PartCategory CreateFromResource(
            PartCategoryResource resource, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(
            PartCategory entity, 
            PartCategoryResource updateResource, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<PartCategory, bool>> SearchExpression(string searchTerm)
        {
            return c => c.Category.Contains(searchTerm.ToUpper()) || c.Description.Contains(searchTerm.ToUpper());
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            PartCategory entity,
            PartCategoryResource resource,
            PartCategoryResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            PartCategory entity, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
