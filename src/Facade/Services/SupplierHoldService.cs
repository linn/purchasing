﻿namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class SupplierHoldService : ISupplierHoldService
    {
        private readonly ISupplierService domainService;

        private readonly IDatabaseService databaseService;

        private readonly ITransactionManager transactionManager;

        private readonly IBuilder<Supplier> supplierResourceBuilder;

        public SupplierHoldService(
            ISupplierService domainService,
            IDatabaseService databaseService,
            ITransactionManager transactionManager,
            IBuilder<Supplier> supplierResourceBuilder)
        {
            this.domainService = domainService;
            this.databaseService = databaseService;
            this.transactionManager = transactionManager;
            this.supplierResourceBuilder = supplierResourceBuilder;
        }

        public IResult<SupplierResource> ChangeSupplierHoldStatus(
            SupplierHoldChangeResource resource, 
            IEnumerable<string> privileges)
        {
            var holdHistoryEntry = new SupplierOrderHoldHistoryEntry
                                       {
                                            PutOnHoldBy = resource.PutOnHoldBy,
                                            ReasonOffHold = resource.ReasonOffHold,
                                            ReasonOnHold = resource.ReasonOnHold,
                                            SupplierId = resource.SupplierId,
                                            TakenOffHoldBy = resource.TakenOffHoldBy
                                       };
            try
            {
                if (!string.IsNullOrEmpty(resource.ReasonOnHold))
                {
                    holdHistoryEntry.Id = this.databaseService.GetIdSequence("SOHH_SEQ");
                }

                var enumerable = privileges as string[] ?? privileges.ToArray();
                var result = this.domainService.ChangeSupplierHoldStatus(holdHistoryEntry, enumerable);
                this.transactionManager.Commit();
                return new SuccessResult<SupplierResource>((SupplierResource)this.supplierResourceBuilder.Build(result, enumerable));
            }
            catch (Exception e)
            {
                return new BadRequestResult<SupplierResource>(e.Message);
            }
        }
    }
}
