namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Edi;
    using Linn.Purchasing.Resources;

    public class EdiSupplierResourceBuilder : IBuilder<EdiSupplier>
    {
        public EdiSupplierResource BuildResource(EdiSupplier model, IEnumerable<string> claims)
        {
            return new EdiSupplierResource
                       {
                           SupplierId = model.SupplierId,
                           SupplierName = model.SupplierName,
                           VendorManager = model.VendorManager,
                           VendorManagerName = model.VendorManagerName,
                           EdiEmailAddress = model.EdiEmailAddress,
                           NumOrders = model.NumOrders
                       };
        }

        object IBuilder<EdiSupplier>.Build(EdiSupplier model, IEnumerable<string> claims) => this.BuildResource(model, claims);

        public string GetLocation(EdiSupplier model)
        {
            throw new NotImplementedException();
        }
    }
}
