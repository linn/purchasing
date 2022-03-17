namespace Linn.Purchasing.Service.Modules.Reports
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Purchasing.Facade.Services;

    using Microsoft.AspNetCore.Http;

    public class PartsReceivedReportModule : CarterModule
    {
        private readonly ITqmsJobRefService tqmsJobRefService;

        public PartsReceivedReportModule(ITqmsJobRefService tqmsJobRefService)
        {
            this.tqmsJobRefService = tqmsJobRefService;
            this.Get("/purchasing/tqms-jobrefs", this.GetJobRefs);
        }

        private async Task GetJobRefs(HttpRequest request, HttpResponse response)
        {
            var result = this.tqmsJobRefService.GetMostRecentJobRefs(50);
            await response.Negotiate(result);
        }
    }
}
