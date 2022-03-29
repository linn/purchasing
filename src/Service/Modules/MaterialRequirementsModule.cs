﻿namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources.MaterialRequirements;
    using Linn.Purchasing.Resources.SearchResources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Http;

    public class MaterialRequirementsModule : CarterModule
    {
        private readonly IMaterialRequirementsPlanningFacadeService materialRequirementsPlanningFacadeService;

        private readonly IFacadeResourceFilterService<MrpRunLog, int, MrpRunLogResource, MrpRunLogResource, MaterialRequirementsSearchResource> mrpRunLogFacadeService;

        public MaterialRequirementsModule(
            IFacadeResourceFilterService<MrpRunLog, int, MrpRunLogResource, MrpRunLogResource, MaterialRequirementsSearchResource> mrpRunLogFacadeService,
            IMaterialRequirementsPlanningFacadeService materialRequirementsPlanningFacadeService)
        {
            this.materialRequirementsPlanningFacadeService = materialRequirementsPlanningFacadeService;
            this.mrpRunLogFacadeService = mrpRunLogFacadeService;
            this.Get("/purchasing/material-requirements/run-logs", this.GetAllRunLogs);
            this.Get("/purchasing/material-requirements/run-logs/{id:int}", this.GetRunLogById);
            this.Post("/purchasing/material-requirements/run-mrp", this.RunMrp);
        }

        private async Task RunMrp(HttpRequest request, HttpResponse response)
        {
            await response.Negotiate(
                this.materialRequirementsPlanningFacadeService.RunMrp(request.HttpContext.GetPrivileges()));
        }

        private async Task GetRunLogById(HttpRequest req, HttpResponse res)
        {
            var id = req.RouteValues.As<int>("id");

            var result = this.mrpRunLogFacadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetAllRunLogs(HttpRequest req, HttpResponse res)
        {
            var searchTerm = req.Query.As<string>("searchTerm");
            if (string.IsNullOrEmpty(searchTerm))
            {
                var result = this.mrpRunLogFacadeService.GetAll(req.HttpContext.GetPrivileges());
                await res.Negotiate(result);
            }
            else
            {
                var result = this.mrpRunLogFacadeService.FindBy(
                    new MaterialRequirementsSearchResource { JobRef = searchTerm },
                    req.HttpContext.GetPrivileges());
                await res.Negotiate(result);
            }
        }
    }
}
