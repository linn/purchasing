namespace Linn.Purchasing.Service.Modules
{
    using System;
    using System.Threading.Tasks;

    using Carter;
    using Carter.ModelBinding;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Http;

    public class ThingModule : CarterModule
    {
        private readonly IFacadeResourceService2<Thing, int, ThingResource, ThingResource> thingFacadeService;

        private readonly IThingService thingService;

        public ThingModule(IFacadeResourceService2<Thing, int, ThingResource, ThingResource> thingFacadeService, IThingService thingService)
        {
            this.thingFacadeService = thingFacadeService;
            this.thingService = thingService;
            this.Get("/purchasing/things", this.GetThings);
            this.Get("/purchasing/things/{id:int}", this.GetThingById);
            this.Post("/purchasing/things/{id:int}", this.DoNothing);
            this.Post("/purchasing/things/send-message", this.SendMessage);
            this.Post("/purchasing/things", this.CreateThing);
            this.Put("/purchasing/things/{id:int}", this.UpdateThing);
        }

        private async Task SendMessage(HttpRequest req, HttpResponse res)
        {
            this.thingService.SendThingMessage("Test Message");

            await res.Negotiate(new SuccessResult<ProcessResultResource>(new ProcessResultResource(true, "ok")));
        }

        private Task DoNothing(HttpRequest req, HttpResponse res)
        {
            throw new NotImplementedException("This should never be hit");
        }

        private async Task GetThings(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(this.thingFacadeService.GetAll());
        }

        private async Task GetThingById(HttpRequest req, HttpResponse res)
        {
            var thingId = req.RouteValues.As<int>("id");

            var result = this.thingFacadeService.GetById(thingId, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task CreateThing(HttpRequest request, HttpResponse response)
        {
            var resource = await request.Bind<ThingResource>();
            var result = this.thingFacadeService.Add(resource);

            await response.Negotiate(result);
        }

        private async Task UpdateThing(HttpRequest request, HttpResponse response)
        {
            var id = request.RouteValues.As<int>("id");
            var resource = await request.Bind<ThingResource>();
            var result = this.thingFacadeService.Update(id, resource);

            await response.Negotiate(result);
        }
    }
}
