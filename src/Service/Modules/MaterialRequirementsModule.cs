namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Http;

    public class MaterialRequirementsModule : CarterModule
    {
        private readonly IFacadeResourceService<MrpRunLog, int, MrpRunLogResource, MrpRunLogResource> mrpRunLogFacadeService;

        public MaterialRequirementsModule(IFacadeResourceService<MrpRunLog, int, MrpRunLogResource, MrpRunLogResource> mrpRunLogFacadeService)
        {
            this.mrpRunLogFacadeService = mrpRunLogFacadeService;
            this.Get("/purchasing/material-requirements/run-logs", this.GetAllRunLogs);
            this.Get("/purchasing/material-requirements/run-logs/{id:int}", this.GetRunLogById);
        }

        private async Task GetRunLogById(HttpRequest req, HttpResponse res)
        {
            var id = req.RouteValues.As<int>("id");

            var result = this.mrpRunLogFacadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetAllRunLogs(HttpRequest req, HttpResponse res)
        {
            var result = this.mrpRunLogFacadeService.GetAll(req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }
    }
}
