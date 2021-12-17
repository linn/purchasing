namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    public class ThingFacadeService : FacadeResourceService<Thing, int, ThingResource, ThingResource>
    {
        private readonly IThingService domainService;

        public ThingFacadeService(
            IRepository<Thing, int> repository, 
            ITransactionManager transactionManager, 
            IBuilder<Thing> resourceBuilder, 
            IThingService domainService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.domainService = domainService;
        }

        protected override Thing CreateFromResource(ThingResource resource)
        {
            var thing = new Thing
                            {
                                Id = resource.Id,
                                Name = resource.Name,
                                RecipientAddress = resource.RecipientAddress,
                                RecipientName = resource.RecipientName,
                                CodeId = resource.Code.Code,
                                Details = resource.Details.Select(
                                    x => new ThingDetail
                                             {
                                                 DetailId = x.DetailId,
                                                 Description = x.Description,
                                             }).ToList()
                            };

            return this.domainService.CreateThing(thing);
        }

        protected override void DeleteOrObsoleteResource(Thing entity)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            Thing entity,
            ThingResource resource,
            ThingResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(Thing entity, ThingResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Thing, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
