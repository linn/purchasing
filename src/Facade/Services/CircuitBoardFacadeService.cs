namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            return new CircuitBoard
                       {
                           BoardCode = resource.BoardCode,
                           Description = resource.Description,
                           ChangeId = null,
                           ChangeState = "LIVE",
                           SplitBom = resource.SplitBom,
                           DefaultPcbNumber = resource.DefaultPcbNumber,
                           VariantOfBoardCode = resource.VariantOfBoardCode,
                           LoadDirectory = resource.LoadDirectory,
                           BoardsPerSheet = resource.BoardsPerSheet,
                           CoreBoard = resource.CoreBoard,
                           ClusterBoard = resource.ClusterBoard,
                           IdBoard = resource.IdBoard
                       };
        }

        protected override void UpdateFromResource(CircuitBoard entity, CircuitBoardResource updateResource, IEnumerable<string> privileges = null)
        {
            entity.ClusterBoard = updateResource.ClusterBoard;
            entity.CoreBoard = updateResource.CoreBoard;
            entity.IdBoard = updateResource.IdBoard;
            entity.Description = updateResource.Description;
            entity.DefaultPcbNumber = updateResource.DefaultPcbNumber;
            entity.SplitBom = updateResource.SplitBom;
            entity.VariantOfBoardCode = updateResource.VariantOfBoardCode;
            entity.Layouts = updateResource.Layouts?.Select(
                a => new BoardLayout
                         {
                             BoardCode = a.BoardCode,
                             LayoutCode = a.LayoutCode,
                             LayoutSequence = a.LayoutSequence,
                             PcbNumber = a.PcbNumber,
                             LayoutType = a.LayoutType,
                             LayoutNumber = a.LayoutNumber,
                             PcbPartNumber = a.PcbPartNumber,
                             ChangeId = a.ChangeId,
                             ChangeState = a.ChangeState
                         }).ToList();
        }

        protected override Expression<Func<CircuitBoard, bool>> SearchExpression(string searchTerm)
        {
            return a => a.BoardCode.Contains(searchTerm) || a.Description.Contains(searchTerm);
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
