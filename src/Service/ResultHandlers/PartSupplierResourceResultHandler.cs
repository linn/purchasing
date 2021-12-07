namespace Linn.Purchasing.Service.ResultHandlers
{
    using System;
    using System.Linq;

    using Linn.Common.Facade.Carter.Handlers;
    using Linn.Purchasing.Resources;

    public class PartSupplierResourceResultHandler : JsonResultHandler<PartSupplierResource>
    {
        public override Func<PartSupplierResource, string> GenerateLocation => r => r.Links.FirstOrDefault(l => l.Rel == "self")?.Href;
    }
}