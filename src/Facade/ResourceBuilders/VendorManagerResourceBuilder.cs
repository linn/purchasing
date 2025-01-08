namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class VendorManagerResourceBuilder : IBuilder<VendorManager>
    {
        public VendorManagerResource Build(VendorManager entity, IEnumerable<string> claims)
        {
            return new VendorManagerResource
            {
                VmId = entity.Id,
                Name = entity.Employee.FullName,
                UserNumber = entity.UserNumber,
                PmMeasured = entity.PmMeasured,
                Links = this.BuildLinks(entity, claims).ToArray()
            };
        }

        public string GetLocation(VendorManager v)
        {
            return $"/purchasing/vendor-managers/{v.Id}";
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
