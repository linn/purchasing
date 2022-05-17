namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public class PurchaseOrderDeliveryFacadeService : IPurchaseOrderDeliveryFacadeService
    {
        private readonly IBuilder<PurchaseOrderDelivery> resourceBuilder;

        private readonly ITransactionManager transactionManager;

        private readonly IPurchaseOrderDeliveryService domainService;

        public PurchaseOrderDeliveryFacadeService(
            IBuilder<PurchaseOrderDelivery> resourceBuilder,
            IPurchaseOrderDeliveryService domainService,
            ITransactionManager transactionManager)
        {
            this.resourceBuilder = resourceBuilder;
            this.domainService = domainService;
            this.transactionManager = transactionManager;
        }

        public IResult<IEnumerable<PurchaseOrderDeliveryResource>> SearchDeliveries(
            string supplierSearchTerm, string orderNumberSearchTerm, bool includeAcknowledged)
        {
            var results = this.domainService.SearchDeliveries(
                supplierSearchTerm,
                orderNumberSearchTerm,
                includeAcknowledged);
            return new SuccessResult<IEnumerable<PurchaseOrderDeliveryResource>>(
                results.Select(x => (PurchaseOrderDeliveryResource)this.resourceBuilder.Build(x, null)));
        }

        public IResult<PurchaseOrderDeliveryResource> PatchDelivery(
            PurchaseOrderDeliveryKey key,
            PatchRequestResource<PurchaseOrderDeliveryResource> requestResource, 
            IEnumerable<string> privileges)
        {
            var privilegesList = privileges.ToList();
            var entity = this.domainService.UpdateDelivery(
                key,
                BuildEntityFromResourceHelper(requestResource.From),
                BuildEntityFromResourceHelper(requestResource.To),
                privilegesList);

            this.transactionManager.Commit();

            return new SuccessResult<PurchaseOrderDeliveryResource>(
                (PurchaseOrderDeliveryResource)this.resourceBuilder.Build(entity, privilegesList));
        }
        
        public IResult<BatchUpdateProcessResultResource> BatchUpdateDeliveriesFromCsv(
            string csvString, IEnumerable<string> privileges)
        {
            var reader = new StringReader(csvString);
            var changes = new List<PurchaseOrderDeliveryUpdate>();
            try
            {
                while (reader.ReadLine() is { } line)
                {
                    // assuming csv lines are in the form <orderNumber>,<delivery-no>,<newAdvisedDate>,<newReason>
                    var row = line.Split(",");

                    if (!int.TryParse(
                            new string(row[0].Trim().Where(char.IsDigit).ToArray()), // strip out non numeric chars 
                            out var orderNumber))
                    {
                        throw new InvalidOperationException($"Invalid Order Number: {row[0]}.");
                    }

                    if (!int.TryParse(row[1].Trim(), out var delNo))
                    {
                        throw new InvalidOperationException($"Invalid Delivery Number: {row[0]} = {row[1]}.");
                    }

                    var firstFormatSatisfied =
                        DateTime.TryParseExact(row[2]
                            .Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate);
                    var secondFormatSatisfied =
                        DateTime.TryParseExact(row[2]
                            .Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);

                    // only supports two date formats for now, i.e.  31/01/2000 and 31-jan-2000
                    if (
                        !firstFormatSatisfied
                        &&
                        !secondFormatSatisfied)
                    {
                        throw new InvalidOperationException($"Date format not recognised for {row[2]}.");
                    }
                    
                    changes.Add(new PurchaseOrderDeliveryUpdate
                                    {
                                        Key = new PurchaseOrderDeliveryKey
                                                  {
                                                      OrderNumber = orderNumber,
                                                      OrderLine = 1, // hardcoded for now
                                                      DeliverySequence = delNo // hardcoded for now since we can't handle split deliveries yet
                                                  },
                                        NewDateAdvised = parsedDate,
                                        NewReason = row[3].Trim()
                                    });
                }

                var result = this.domainService.BatchUpdateDeliveries(changes, privileges);
                this.transactionManager.Commit();

                return new SuccessResult<BatchUpdateProcessResultResource>(
                    new BatchUpdateProcessResultResource
                        {
                            Success = result.Success,
                            Message = result.Message,
                            Errors = result.Errors?.Select(e => new ErrorResource
                                                                   {
                                                                       Descriptor = e.Descriptor,
                                                                       Message = e.Message
                                                                   })
                        });
            }
            catch (Exception e)
            {
                return new BadRequestResult<BatchUpdateProcessResultResource>(e.Message);
            }
        }

        private static PurchaseOrderDelivery BuildEntityFromResourceHelper(PurchaseOrderDeliveryResource resource)
        {
            return new PurchaseOrderDelivery
                       {
                           DateAdvised = string.IsNullOrEmpty(resource.DateAdvised) 
                                             ? null : DateTime.Parse(resource.DateAdvised),
                           AvailableAtSupplier = resource.AvailableAtSupplier,
                           RescheduleReason = resource.RescheduleReason,
                           SupplierConfirmationComment = resource.SupplierConfirmationComment
                       };
        }
    }
}
