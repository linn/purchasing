﻿namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Resources.Boms;

    public interface IBomFacadeService
    {
        IResult<BomTreeNode> PostBom(PostBomResource node);

        IResult<BomTreeNode> CopyBom(string srcPartNumber, int destBomId, string destPartNumber);
    }
}
