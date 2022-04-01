namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;

    public interface IMaterialRequirementsPlanningFacadeService
    {
        IResult<ProcessResultResource> RunMrp(IEnumerable<string> privileges);
    }
}
