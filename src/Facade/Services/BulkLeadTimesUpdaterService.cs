namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class BulkLeadTimesUpdaterService : IBulkLeadTimesUpdaterService
    {
        private readonly IPartSupplierService domainService;

        private readonly ITransactionManager transactionManager;

        public BulkLeadTimesUpdaterService(
            IPartSupplierService domainService,
            ITransactionManager transactionManager)
        {
            this.domainService = domainService;
            this.transactionManager = transactionManager;
        }

        public IResult<ProcessResultResource> BulkUpdateFromCsv(string csvString)
        {
            var reader = new StringReader(csvString);
            var changes = new List<LeadTimeUpdateModel>();
            try
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var row = line.Split(",");
                    changes.Add(new LeadTimeUpdateModel(row[0], row[1]));
                }

                var result = this.domainService.BulkUpdateLeadTimes(changes);
                this.transactionManager.Commit();

                return new SuccessResult<ProcessResultResource>(
                    new ProcessResultResource(result.Success, result.Message));
            }
            catch (Exception e)
            {
                return new BadRequestResult<ProcessResultResource>(e.Message);
            }
        }
    }
}
