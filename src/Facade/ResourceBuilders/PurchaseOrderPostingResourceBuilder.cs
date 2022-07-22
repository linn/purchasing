namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class PurchaseOrderPostingResourceBuilder : IBuilder<PurchaseOrderPosting>
    {
        public PurchaseOrderPostingResource Build(PurchaseOrderPosting entity, IEnumerable<string> claims)
        {
            return new PurchaseOrderPostingResource
                       {
                           Building = entity.Building,
                           Id = entity.Id,
                           LineNumber = entity.LineNumber,
                           NominalAccount =
                               new NominalAccountResource
                                   {
                                       AccountId = entity.NominalAccountId,
                                       Department = new DepartmentResource
                                                        {
                                                            Description =
                                                                entity.NominalAccount?.Department?.Description,
                                                            DepartmentCode = entity.NominalAccount?.Department
                                                                ?.DepartmentCode
                                                        },
                                       Nominal = new NominalResource
                                                     {
                                                         Description = entity.NominalAccount?.Nominal?.Description,
                                                         NominalCode = entity.NominalAccount?.Nominal?.NominalCode
                                                     }
                                   },
                           NominalAccountId = entity.NominalAccountId,
                           Notes = entity.Notes,
                           OrderNumber = entity.OrderNumber,
                           Person = entity.Person,
                           Product = entity.Product,
                           Qty = entity.Qty,
                           Vehicle = entity.Vehicle
                       };
        }

        public string GetLocation(PurchaseOrderPosting p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<PurchaseOrderPosting>.Build(PurchaseOrderPosting entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }
    }
}
