namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class SigningLimitFacadeService : FacadeResourceService<SigningLimit, int, SigningLimitResource, SigningLimitResource>
    {
        public SigningLimitFacadeService(
            IRepository<SigningLimit, int> repository,
            ITransactionManager transactionManager,
            IBuilder<SigningLimit> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override SigningLimit CreateFromResource(SigningLimitResource resource)
        {
            var signingLimit = new SigningLimit
                                   {
                                       ProductionLimit = resource.ProductionLimit,
                                       SundryLimit = resource.SundryLimit,
                                       UserNumber = resource.User.Id,
                                       ReturnsAuthorisation = resource.ReturnsAuthorisation,
                                       Unlimited = resource.Unlimited
                                   };

            return signingLimit;
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
