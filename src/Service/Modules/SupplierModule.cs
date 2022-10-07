namespace Linn.Purchasing.Service.Modules
{
    using System.IO;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class SupplierModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/suppliers", this.SearchSuppliers);
            app.MapGet("/purchasing/suppliers/{id:int}", this.GetSupplier);
            app.MapPut("/purchasing/suppliers/{id:int}", this.UpdateSupplier);
            app.MapPost("/purchasing/suppliers", this.CreateSupplier);

            app.MapGet("/purchasing/part-suppliers/record", this.GetPartSupplierRecord);
            app.MapPut("/purchasing/part-suppliers/record", this.UpdatePartSupplier);
            app.MapGet("/purchasing/part-suppliers", this.SearchPartSuppliers);
            app.MapPost("/purchasing/part-suppliers/record", this.CreatePartSupplier);

            app.MapGet("/purchasing/part-suppliers/application-state", this.GetPartSuppliersState);
            app.MapGet("/purchasing/suppliers/application-state", this.GetSuppliersState);
            app.MapPost("/purchasing/preferred-supplier-changes", this.CreatePreferredSupplierChange);
            app.MapGet("/purchasing/price-change-reasons", this.GetPriceChangeReasons);
            app.MapGet("/purchasing/part-suppliers/part-price-conversions", this.GetPartPriceConversions);
            app.MapPost("/purchasing/suppliers/bulk-lead-times", this.UploadBulkLeadTimes);
            app.MapGet("/purchasing/part-categories/", this.SearchPartCategories);

            app.MapPost("/purchasing/suppliers/hold", this.ChangeHoldStatus);
            app.MapGet("/purchasing/suppliers/planners", this.GetPlanners);
        }

        private async Task UploadBulkLeadTimes(
            HttpRequest req,
            HttpResponse res,
            IBulkLeadTimesUpdaterService bulkLeadTimesUpdaterService,
            int? groupId,
            int supplierId)
        {
            var reader = new StreamReader(req.Body).ReadToEndAsync();

            var result = bulkLeadTimesUpdaterService.BulkUpdateFromCsv(
                supplierId,
                reader.Result, 
                req.HttpContext.GetPrivileges(),
                groupId);

            await res.Negotiate(result);
        }

        private async Task GetSupplier(
            HttpRequest req,
            HttpResponse res,
            int id,
            IFacadeResourceService<Supplier, int, SupplierResource, SupplierResource> supplierFacadeService)
        {
            var result = supplierFacadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task SearchSuppliers(
            HttpRequest req,
            HttpResponse res,
            string searchTerm,
            IFacadeResourceService<Supplier, int, SupplierResource, SupplierResource> supplierFacadeService)
        {
            var result = supplierFacadeService.Search(searchTerm);

            await res.Negotiate(result);
        }

        private async Task UpdateSupplier(
            HttpRequest req,
            HttpResponse res,
            int id,
            SupplierResource resource,
            IFacadeResourceService<Supplier, int, SupplierResource, SupplierResource> supplierFacadeService)
        {
            var result = supplierFacadeService.Update(id, resource, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetPartSupplierRecord(
            HttpRequest req,
            HttpResponse res,
            int partId,
            int supplierId,
            IPartFacadeService partFacadeService,
            IFacadeResourceFilterService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource> partSupplierFacadeService)
        {
            var partNumber = partFacadeService.GetPartNumberFromId(partId);

            var result = partSupplierFacadeService.GetById(
                new PartSupplierKey
                    {
                        PartNumber = partNumber,
                        SupplierId = supplierId
                },
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task UpdatePartSupplier(
            HttpRequest req,
            HttpResponse res,
            PartSupplierResource resource,
            IFacadeResourceFilterService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource> partSupplierFacadeService)
        {
            var key = new PartSupplierKey { PartNumber = resource.PartNumber, SupplierId = resource.SupplierId };

            var result = partSupplierFacadeService.Update(
                key,
                resource,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task SearchPartSuppliers(
            HttpRequest req,
            HttpResponse res,
            string partNumber,
            string supplierName,
            IFacadeResourceFilterService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource> partSupplierFacadeService)
        {
            var result = partSupplierFacadeService.FilterBy(
                new PartSupplierSearchResource
                    {
                        PartNumberSearchTerm = partNumber,
                        SupplierNameSearchTerm = supplierName
                    },
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetPartSuppliersState(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceFilterService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource> partSupplierFacadeService)
        {
            var privileges = req.HttpContext.GetPrivileges();

            var result = partSupplierFacadeService.GetApplicationState(privileges);

            await res.Negotiate(result);
        }

        private async Task GetSuppliersState(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<Supplier, int, SupplierResource, SupplierResource> supplierFacadeService)
        {
            var privileges = req.HttpContext.GetPrivileges();

            var result = supplierFacadeService.GetApplicationState(privileges);

            await res.Negotiate(result);
        }

        private async Task CreatePartSupplier(
            HttpRequest req,
            HttpResponse res,
            PartSupplierResource resource,
            IFacadeResourceFilterService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource> partSupplierFacadeService)
        {
            var result = partSupplierFacadeService.Add(
                resource,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task CreateSupplier(
            HttpRequest req,
            HttpResponse res,
            SupplierResource resource,
            IFacadeResourceService<Supplier, int, SupplierResource, SupplierResource> supplierFacadeService)
        {
            resource.OpenedById = req.HttpContext.User.GetEmployeeNumber();
            var result = supplierFacadeService.Add(
                resource,
                req.HttpContext.GetPrivileges(),
                null);

            await res.Negotiate(result);
        }

        private async Task CreatePreferredSupplierChange(
            HttpRequest req,
            HttpResponse res,
            PreferredSupplierChangeResource resource,
            IFacadeResourceService<PreferredSupplierChange, PreferredSupplierChangeKey, PreferredSupplierChangeResource, PreferredSupplierChangeKey> preferredSupplierChangeService)
        {
            var privileges = req.HttpContext.GetPrivileges();
            var result = preferredSupplierChangeService.Add(
                resource,
                privileges);

            await res.Negotiate(result);
        }

        private async Task GetPriceChangeReasons(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<PriceChangeReason, string, PriceChangeReasonResource, PriceChangeReasonResource> priceChangeReasonService)
        {
            var result = priceChangeReasonService.GetAll();

            await res.Negotiate(result);
        }

        private async Task GetPartPriceConversions(
            HttpRequest req,
            HttpResponse res,
            string partNumber,
            string newCurrency,
            decimal newPrice,
            string ledger,
            string round,
            IPartFacadeService partFacadeService)
        {
            var result = partFacadeService.GetPrices(
                partNumber,
                newCurrency,
                newPrice,
                ledger,
                round);

            await res.Negotiate(result);
        }

        private async Task SearchPartCategories(
            HttpRequest req,
            HttpResponse res,
            string searchTerm,
            IFacadeResourceService<PartCategory, string, PartCategoryResource, PartCategoryResource> partCategoryService)
        {
            var result = partCategoryService.Search(searchTerm);

            await res.Negotiate(result);
        }

        private async Task ChangeHoldStatus(
            HttpRequest req,
            HttpResponse res,
            SupplierHoldChangeResource resource,
            ISupplierHoldService supplierHoldService)
        {
            var result = supplierHoldService.ChangeSupplierHoldStatus(
                resource,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetPlanners(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<Planner, int, PlannerResource, PlannerResource> plannerService)
        {
            var result = plannerService.GetAll();

            await res.Negotiate(result);
        }
    }
}
