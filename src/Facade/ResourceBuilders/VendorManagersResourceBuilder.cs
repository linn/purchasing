namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    public class VendorManagersResourceBuilder : IBuilder<IEnumerable<VendorManager>>
    {
        public IEnumerable<VendorManagerResource> Build(
            IEnumerable<VendorManager> entityList,
            IEnumerable<string> claims)
        {
            return entityList.Select(
                e => new VendorManagerResource
                         {
                             VmId = e.VmId, Name = e.Employee.FullName, UserNumber = e.UserNumber
                         });
        }

        public string GetLocation(IEnumerable<VendorManager> p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<IEnumerable<VendorManager>>.Build(
            IEnumerable<VendorManager> entityList,
            IEnumerable<string> claims)
        {
            return this.Build(entityList, claims);
        }
    }
}
