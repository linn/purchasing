namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

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

        public IResult<BatchUpdateProcessResultResourceWithLinks> BulkUpdateFromCsv(
            int supplierId,
            string csvString,
            IEnumerable<string> privileges,
            int? groupId = null)
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

                var result = this.domainService.BulkUpdateLeadTimes(supplierId, changes, privileges, groupId);
                this.transactionManager.Commit();

                return new SuccessResult<BatchUpdateProcessResultResourceWithLinks>(
                    new BatchUpdateProcessResultResourceWithLinks
                        {
                            Message = result.Message,
                            Success = result.Success,
                            Errors = result.Errors?.Select(x => new ErrorResource
                                                                   {
                                                                        Descriptor = x.Descriptor,
                                                                        Message = x.Message
                                                                   })
                        });
            }
            catch (Exception e)
            {
                return new BadRequestResult<BatchUpdateProcessResultResourceWithLinks>(e.Message);
            }
        }
    }
}
