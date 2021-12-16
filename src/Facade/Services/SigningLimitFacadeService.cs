namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class SigningLimitFacadeService : FacadeResourceService2<SigningLimit, int, SigningLimitResource, SigningLimitResource>
    {
        private readonly IRepository<SigningLimitLog, int> logRepository;

        public SigningLimitFacadeService(
            IRepository<SigningLimit, int> repository,
            ITransactionManager transactionManager,
            IBuilder<SigningLimit> resourceBuilder,
            IRepository<SigningLimitLog, int> logRepository)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.logRepository = logRepository;
        }

        protected override SigningLimit CreateFromResource(SigningLimitResource resource)
        {
            var signingLimit = new SigningLimit
                                   {
                                       ProductionLimit = resource.ProductionLimit,
                                       SundryLimit = resource.SundryLimit,
                                       UserNumber = resource.UserNumber,
                                       ReturnsAuthorisation = resource.ReturnsAuthorisation,
                                       Unlimited = resource.Unlimited
                                   };

            return signingLimit;
        }

        protected override void DeleteOrObsoleteResource(SigningLimit entity)
        {
            this.RemoveFromDatabase(entity);
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            SigningLimit entity,
            SigningLimitResource resource,
            SigningLimitResource updateResource)
        {
            var log = new SigningLimitLog
                          {
                              LogAction = actionType,
                              LogTime = DateTime.UtcNow,
                              LogUserNumber = userNumber,
                              ProductionLimit = entity.ProductionLimit,
                              ReturnsAuthorisation = entity.ReturnsAuthorisation,
                              SundryLimit = entity.SundryLimit,
                              Unlimited = entity.Unlimited,
                              UserNumber = entity.UserNumber
                          };
            this.logRepository.Add(log);
        }

        protected override void UpdateFromResource(SigningLimit entity, SigningLimitResource updateResource)
        {
            entity.ProductionLimit = updateResource.ProductionLimit;
            entity.SundryLimit = updateResource.SundryLimit;
            entity.Unlimited = updateResource.Unlimited;
            entity.ReturnsAuthorisation = updateResource.ReturnsAuthorisation;
        }

        protected override Expression<Func<SigningLimit, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
