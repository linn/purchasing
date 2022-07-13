namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Edi;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public class EdiOrdersFacadeService : IEdiOrdersFacadeService
    {
        private readonly IEdiOrderService domainService;

        private readonly IBuilder<EdiOrder> builder;

        private readonly IBuilder<EdiSupplier> supplierBuilder;

        public EdiOrdersFacadeService(IEdiOrderService domainService, IBuilder<EdiOrder> builder, IBuilder<EdiSupplier> supplierBuilder)
        {
            this.domainService = domainService;
            this.builder = builder;
            this.supplierBuilder = supplierBuilder;
        }

        public IResult<IEnumerable<EdiOrderResource>> GetEdiOrders(int supplierId)
        {
            var orders = this.domainService.GetEdiOrders(supplierId);
            var resources = orders.Select(o => (EdiOrderResource)this.builder.Build(o, null));
            return new SuccessResult<IEnumerable<EdiOrderResource>>(resources);
        }

        public IResult<IEnumerable<EdiSupplierResource>> GetEdiSuppliers()
        {
            var suppliers = this.domainService.GetEdiSuppliers();
            var resources = suppliers.Select(s => (EdiSupplierResource)this.supplierBuilder.Build(s, null));
            return new SuccessResult<IEnumerable<EdiSupplierResource>>(resources);
        }

        public IResult<ProcessResultResource> SendEdiOrder(SendEdiEmailResource resource)
        {
            var processResult = this.domainService.SendEdiOrder(
                resource.supplierId,
                resource.altEmail,
                resource.additionalEmail,
                resource.additionalText,
                resource.test);

            return new SuccessResult<ProcessResultResource>(new ProcessResultResource
                                                                {
                                                                    Message = processResult.Message,
                                                                    Success = processResult.Success
                                                                });
        }
    }
}
