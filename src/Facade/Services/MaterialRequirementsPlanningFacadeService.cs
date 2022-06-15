﻿namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources;

    public class MaterialRequirementsPlanningFacadeService : IMaterialRequirementsPlanningFacadeService
    {
        private readonly IMaterialRequirementsPlanningService materialRequirementsPlanningService;

        public MaterialRequirementsPlanningFacadeService(IMaterialRequirementsPlanningService materialRequirementsPlanningService)
        {
            this.materialRequirementsPlanningService = materialRequirementsPlanningService;
        } 

        public IResult<ProcessResultResource> RunMrp(IEnumerable<string> privileges)
        {
            var result = this.materialRequirementsPlanningService.RunMrp();

            if (result.Success)
            {
                return new SuccessResult<ProcessResultResource>(
                    new ProcessResultResource(result.Success, result.Message)
                        {
                            Links = new LinkResource[] { new LinkResource("status", result.ProcessHref) }
                        });
            }

            return new BadRequestResult<ProcessResultResource>(result.Message);
        }
    }
}
