namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources.Boms;

    public interface ICircuitBoardFacadeService 
        : IFacadeResourceService<CircuitBoard, string, CircuitBoardResource, CircuitBoardComponentsUpdateResource>
    {
        IResult<CircuitBoardResource> UpdateBoardComponents(
            int id,
            CircuitBoardComponentsUpdateResource updateResource,
            IEnumerable<string> privileges = null);
    }
}
