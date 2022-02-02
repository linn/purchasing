namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    public class VendorManagerResourceBuilder : IBuilder<VendorManager>
    {
        public VendorManagerResource Build(VendorManager entity, IEnumerable<string> claims)
        {
            return new VendorManagerResource
            {
                VmId = entity.VmId,
                Name = entity.Employee.FullName,
                UserNumber = entity.UserNumber
            };
        }

        public string GetLocation(VendorManager v)
        {
            return $"/purchasing/vendor-managers/{v.VmId}";
        }

        object IBuilder<VendorManager>.Build(VendorManager entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }

        private IEnumerable<LinkResource> BuildLinks(VendorManager model, IEnumerable<string> claims)
        {
            yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
        }
    }
}
