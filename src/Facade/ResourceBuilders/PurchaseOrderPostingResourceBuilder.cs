namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class PurchaseOrderPostingResourceBuilder : IBuilder<PurchaseOrderPosting>
    {
        private readonly IRepository<NominalAccount, int> nomaccRepository;

        public PurchaseOrderPostingResourceBuilder(IRepository<NominalAccount, int> nomaccRepository)
        {
            this.nomaccRepository = nomaccRepository;
        }

        public PurchaseOrderPostingResource Build(PurchaseOrderPosting entity, IEnumerable<string> claims)
        {
            var nomacc = entity.NominalAccountId.HasValue ? this.nomaccRepository.FindById((int)entity.NominalAccountId)
                             : null;
            return new PurchaseOrderPostingResource
                       {
                           Building = entity.Building,
                           Id = entity.Id,
                           LineNumber = entity.LineNumber,
                           NominalAccount = nomacc != null ? 
                               new NominalAccountResource
                                   {
                                       AccountId = (int)entity.NominalAccountId,
                                       Department = new DepartmentResource
                                                        {
                                                            DepartmentDescription = nomacc.Department?.Description,
                                                            DepartmentCode = nomacc.Department
                                                                ?.DepartmentCode
                                                        },
                                       Nominal = new NominalResource
                                                     {
                                                         Description = nomacc.Nominal?.Description,
                                                         NominalCode = nomacc.Nominal?.NominalCode
                                                     }
                                   } 
                               : null,
                           NominalAccountId = nomacc != null ? (int)entity.NominalAccountId : null,
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
