namespace Linn.Purchasing.Service.ResultHandlers
{
    using Linn.Common.Facade.Carter.Handlers;
    using Linn.Purchasing.Resources;

    public class PurchaseOrderApplicationStateResultHandler : JsonResultHandler<PurchaseOrderResource>
    {
        public PurchaseOrderApplicationStateResultHandler() : base("application/vnd.linn.application-state+json;version=1")
        {
        }
    }
}
