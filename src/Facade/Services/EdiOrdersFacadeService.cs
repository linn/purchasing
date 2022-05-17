namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Edi;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public class EdiOrdersFacadeService : IEdiOrdersFacadeService
    {
        private IEdiOrderService domainService;

        private IBuilder<EdiOrder> builder;

        public EdiOrdersFacadeService(IEdiOrderService domainService, IBuilder<EdiOrder> builder)
        {
            this.domainService = domainService;
            this.builder = builder;
        }

        public IResult<IEnumerable<EdiOrderResource>> GetEdiOrders(int supplierId)
        {
            var orders = this.domainService.GetEdiOrders(supplierId);
            var resources = orders.Select(o => (EdiOrderResource)this.builder.Build(o, null));
            return new SuccessResult<IEnumerable<EdiOrderResource>>(resources);
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
