namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources.Boms;

    public interface ICircuitBoardFacadeService 
        : IFacadeResourceService<CircuitBoard, string, CircuitBoardResource, CircuitBoardComponentsUpdateResource>
    {
        IResult<CircuitBoardResource> UpdateBoardComponents(
            string id,
            CircuitBoardComponentsUpdateResource updateResource,
            IEnumerable<string> privileges = null);

        IResult<ProcessResultResource> UploadBoardFile(
            string boardCode,
            string revisionCode,
            string fileType,
            string fileString,
            int? pcasChangeId,
            bool makeChanges,
            IEnumerable<string> getPrivileges);
    }
}
