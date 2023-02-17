namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Resources;

    public class PartDataSheetValuesService 
        : FacadeResourceService<PartDataSheetValues, PartDataSheetValuesKey, PartDataSheetValuesResource, PartDataSheetValuesResource>
    {
        public PartDataSheetValuesService(IRepository<PartDataSheetValues, PartDataSheetValuesKey> repository, ITransactionManager transactionManager, IBuilder<PartDataSheetValues> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override PartDataSheetValues CreateFromResource(PartDataSheetValuesResource resource, IEnumerable<string> privileges = null)
        {
            return new PartDataSheetValues
                       {
                           AttributeSet = resource.AttributeSet,
                           Field = resource.Field,
                           Value = resource.Value,
                           Description = resource.Description,
                           AssemblyTechnology = resource.AssemblyTechnology,
                           ImdsNumber = resource.ImdsNumber,
                           ImdsWeight = resource.ImdsWeight
                       };
        }

        protected override void UpdateFromResource(
            PartDataSheetValues entity,
            PartDataSheetValuesResource updateResource,
            IEnumerable<string> privileges = null)
        {
            entity.AssemblyTechnology = updateResource.AssemblyTechnology;
            entity.Description = updateResource.Description;
            entity.ImdsNumber = updateResource.ImdsNumber;
            entity.ImdsWeight = updateResource.ImdsWeight;
        }

        protected override Expression<Func<PartDataSheetValues, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            PartDataSheetValues entity,
            PartDataSheetValuesResource resource,
            PartDataSheetValuesResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(PartDataSheetValues entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
