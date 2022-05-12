namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public class PurchaseOrderDeliveryFacadeService : IPurchaseOrderDeliveryFacadeService
    {
        private readonly IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> repository;

        private readonly IBuilder<PurchaseOrderDelivery> resourceBuilder;

        private readonly ITransactionManager transactionManager;

        private readonly IPurchaseOrderDeliveryService domainService;

        public PurchaseOrderDeliveryFacadeService(
            IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> repository,
            IBuilder<PurchaseOrderDelivery> resourceBuilder,
            IPurchaseOrderDeliveryService domainService,
            ITransactionManager transactionManager)
        {
            this.repository = repository;
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
            var entity = this.repository.FindById(key);

            this.domainService.UpdateDelivery(
                key,
                BuildEntityFromResourceHelper(requestResource.From),
                BuildEntityFromResourceHelper(requestResource.To),
                privilegesList);

            if (requestResource.From.DateAdvised != requestResource.To.DateAdvised)
            {
                entity.DateAdvised = string.IsNullOrEmpty(requestResource.To.DateAdvised)
                                         ? null : DateTime.Parse(requestResource.To.DateAdvised);
            }

            if (requestResource.From.RescheduleReason != requestResource.To.RescheduleReason)
            {
                entity.RescheduleReason = requestResource.To.RescheduleReason;
            }

            if (requestResource.From.SupplierConfirmationComment != requestResource.To.SupplierConfirmationComment)
            {
                entity.SupplierConfirmationComment = requestResource.To.SupplierConfirmationComment;
            }

            if (requestResource.From.AvailableAtSupplier != requestResource.To.AvailableAtSupplier)
            {
                entity.AvailableAtSupplier = requestResource.To.AvailableAtSupplier;
            }

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
                    // assuming csv lines are in the form <orderNumber>,<newAdvisedDate>,<newReason>
                    var row = line.Split(",");

                    if (!int.TryParse(new string(row[0].Where(char.IsDigit).ToArray()), out var orderNumber))
                    {
                        throw new InvalidOperationException($"Invalid Order Number: {row[0]}.");
                    }

                    if (
                        !DateTime
                        .TryParseExact(
                            row[1], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate)
                        &&
                        !DateTime
                            .TryParseExact(
                                row[1], "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                    {
                        throw new InvalidOperationException($"Date format not recognised for {orderNumber}.");
                    }
                    
                    changes.Add(new PurchaseOrderDeliveryUpdate
                                    {
                                        Key = new PurchaseOrderDeliveryKey
                                                  {
                                                      OrderNumber = orderNumber,
                                                      OrderLine = 1, // hardcoded for now
                                                      DeliverySequence = 1 // hardcoded for now since we can't handle split deliveries yet
                                                  },
                                        NewDateAdvised = parsedDate,
                                        NewReason = row[2]
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
