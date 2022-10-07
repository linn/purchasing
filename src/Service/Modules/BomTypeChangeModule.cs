namespace Linn.Purchasing.Service.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class BomTypeChangeModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/purchasing/bom-type-change", this.PostBomTypeChange);
        }

        private async Task PostBomTypeChange(
            HttpRequest req,
            HttpResponse res,
            BomTypeChangeResource resource,
            IPartFacadeService partFacadeService)
        {
            resource.ChangedBy = req.HttpContext.User.GetEmployeeNumber();
            var result = partFacadeService.ChangeBomType(
                resource,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }
    }
}
