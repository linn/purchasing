namespace Linn.Purchasing.Service.ResultHandlers
{
    using Linn.Common.Facade.Carter.Handlers;
    using Linn.Purchasing.Resources;

    public class SuppliersApplicationStateResultHandler : JsonResultHandler<SupplierResource>
    {
        public SuppliersApplicationStateResultHandler() : base("application/vnd.linn.application-state+json;version=1")
        {
        }
    }
}
