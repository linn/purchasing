﻿namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Resources.Boms;

    public class BomFacadeService : IBomFacadeService
    {
        private readonly IBomChangeService bomChangeService;

        private readonly ITransactionManager transactionManager;

        private readonly IBomTreeService treeService;

        public BomFacadeService(
            IBomChangeService bomChangeService, 
            ITransactionManager transactionManager,
            IBomTreeService treeService)
        {
            this.bomChangeService = bomChangeService;
            this.transactionManager = transactionManager;
            this.treeService = treeService;
        }

        public IResult<BomTreeNode> PostBom(PostBomResource resource)
        {
            try
            {
                this.bomChangeService.ProcessTreeUpdate(
                    resource.TreeRoot,
                    resource.CrNumber,
                    resource.EnteredBy);
                this.transactionManager.Commit();
                return new SuccessResult<BomTreeNode>(
                    this.treeService.BuildBomTree(resource.TreeRoot.Name, null, false, true));
            }
            catch (DomainException e)
            {
                return new BadRequestResult<BomTreeNode>(e.Message);
            }
        }

        public IResult<BomTreeNode> CopyBom(
            string srcPartNumber, 
            string destPartNumber, 
            int changedBy, 
            int crfNumber, 
            string addOrOverwrite, 
            string rootName)
        {
            this.bomChangeService.CopyBom(srcPartNumber, destPartNumber, changedBy, crfNumber, addOrOverwrite);
            this.transactionManager.Commit();

            // todo - some error handling? Could this domain service throw errors?
            return new SuccessResult<BomTreeNode>(this.treeService.BuildBomTree(rootName, null, false, true));
        }

        public IResult<BomTreeNode> DeleteBom(string bomName, int crfNumber, int changedBy, string rootName)
        {
            this.bomChangeService.DeleteAllFromBom(bomName, crfNumber, changedBy);
            this.transactionManager.Commit();
            var updated = this.treeService.BuildBomTree(rootName, null, false, true);
            return new SuccessResult<BomTreeNode>(updated);
        }

        public IResult<BomTreeNode> ExplodeSubAssembly(
            string bomName, int crfNumber, string subAssembly, int changedBy, string rootName)
        {
            this.bomChangeService.ExplodeSubAssembly(bomName, crfNumber, subAssembly, changedBy);
            this.transactionManager.Commit();
            var updated = this.treeService.BuildBomTree(rootName, null, false, true);
            return new SuccessResult<BomTreeNode>(updated);
        }
    }
}

