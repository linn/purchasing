namespace Linn.Purchasing.Facade.Services
{
    using System;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
  
    public class BomFacadeService : IBomFacadeService
    {
        private readonly IBomChangeService bomChangeService;

        public BomFacadeService(
            IBomChangeService bomChangeService)
        {
            this.bomChangeService = bomChangeService;
        }

        public IResult<BomTreeNode> PostBom(BomTreeNode node)
        {
            throw new NotImplementedException();
        }
    }
}
