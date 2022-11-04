namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources.Boms;

    public class CircuitBoardFacadeService : FacadeResourceService<CircuitBoard, string, CircuitBoardResource, CircuitBoardResource>
    {
        public CircuitBoardFacadeService(
            IRepository<CircuitBoard, string> repository,
            ITransactionManager transactionManager,
            IBuilder<CircuitBoard> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override CircuitBoard CreateFromResource(CircuitBoardResource resource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(CircuitBoard entity, CircuitBoardResource updateResource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<CircuitBoard, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            CircuitBoard entity,
            CircuitBoardResource resource,
            CircuitBoardResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(CircuitBoard entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
