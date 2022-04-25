namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources.MaterialRequirements;
    using Linn.Purchasing.Resources.SearchResources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class MaterialRequirementsModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/material-requirements/last-run", this.GetDetails);
            app.MapGet("/purchasing/material-requirements/run-logs", this.GetAllRunLogs);
            app.MapGet("/purchasing/material-requirements/run-logs/{id:int}", this.GetRunLogById);
            app.MapPost("/purchasing/material-requirements/run-mrp", this.RunMrp);
        }

        private async Task RunMrp(
            HttpRequest request,
            HttpResponse response,
            IMaterialRequirementsPlanningFacadeService materialRequirementsPlanningFacadeService)
        {
            await response.Negotiate(materialRequirementsPlanningFacadeService.RunMrp(request.HttpContext.GetPrivileges()));
        }

        private async Task GetRunLogById(
            HttpRequest req,
            HttpResponse res,
            int id,
            IFacadeResourceFilterService<MrpRunLog, int, MrpRunLogResource, MrpRunLogResource, MaterialRequirementsSearchResource> mrpRunLogFacadeService)
        {
            var result = mrpRunLogFacadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetAllRunLogs(
            HttpRequest req,
            HttpResponse res,
            string searchTerm,
            IMaterialRequirementsPlanningFacadeService materialRequirementsPlanningFacadeService,
            IFacadeResourceFilterService<MrpRunLog, int, MrpRunLogResource, MrpRunLogResource, MaterialRequirementsSearchResource> mrpRunLogFacadeService)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                var result = mrpRunLogFacadeService.GetAll(req.HttpContext.GetPrivileges());
                await res.Negotiate(result);
            }
            else
            {
                var result = mrpRunLogFacadeService.FindBy(
                    new MaterialRequirementsSearchResource { JobRef = searchTerm },
                    req.HttpContext.GetPrivileges());
                await res.Negotiate(result);
            }
        }

        private async Task GetDetails(
            HttpRequest req,
            HttpResponse res,
            ISingleRecordFacadeResourceService<MrMaster, MrMasterResource> masterFacadeService)
        {
            var result = masterFacadeService.Get(req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }
    }
}
