namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class PartDataSheetValuesModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/part-data-sheet-values/", this.GetAll);
            app.MapPut("/purchasing/part-data-sheet-values/{attributeSet}/{field}/{value}", this.PutPartDataSheetValuesEntry);
            app.MapPost("/purchasing/part-data-sheet-values/", this.CreatePartDataSheetValuesEntry);
        }

        private async Task PutPartDataSheetValuesEntry(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<PartDataSheetValues, PartDataSheetValuesKey, PartDataSheetValuesResource, PartDataSheetValuesResource> service,
            string attributeSet, 
            string field,
            string value,
            PartDataSheetValuesResource resource)
        {
            var result = service.Update(
                new PartDataSheetValuesKey
                    {
                        AttributeSet = attributeSet, Field = field, Value = value
                    },
                resource);
            await res.Negotiate(result);
        }

        private async Task CreatePartDataSheetValuesEntry(
            HttpRequest req,
            HttpResponse res,
            PartDataSheetValuesResource resource,
            IFacadeResourceService<PartDataSheetValues, PartDataSheetValuesKey, PartDataSheetValuesResource, PartDataSheetValuesResource> service)
        {
            var result = service.Add(
                resource,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetAll(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<PartDataSheetValues, PartDataSheetValuesKey, PartDataSheetValuesResource, PartDataSheetValuesResource> service)
        {
            await res.Negotiate(service.GetAll());
        }
    }
}
