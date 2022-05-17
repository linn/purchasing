﻿namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Resources.MaterialRequirements;

    public interface IMaterialRequirementsReportFacadeService
    {
        IResult<MrReportResource> GetMaterialRequirements(MrRequestResource request, IEnumerable<string> privileges);
    }
}
