namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Edi;
    using Linn.Purchasing.Resources;

    public class EdiOrderResourceBuilder : IBuilder<EdiOrder>
    {
        public EdiOrderResource BuildResource(EdiOrder model, IEnumerable<string> claims)
        {
            return new EdiOrderResource { OrderNumber = model.OrderNumber, SupplierId = model.SupplierId, Id = model.Id };
        }

        object IBuilder<EdiOrder>.Build(EdiOrder model, IEnumerable<string> claims) => this.BuildResource(model, claims);

        public string GetLocation(EdiOrder model)
        {
            throw new NotImplementedException();
        }
    }
}
