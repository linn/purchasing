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
                            .Trim(), "dd'/'M'/'yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate1);
                    var secondFormatSatisfied =
                        DateTime.TryParseExact(row[2]
                            .Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate2);

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
                                                      DeliverySequence = delNo
                                                  },
                                        NewDateAdvised =firstFormatSatisfied ? parsedDate1 : parsedDate2,
                                        NewReason = row.Length < 4 ? null : row[3].Trim()
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

        public IResult<IEnumerable<PurchaseOrderDeliveryResource>> UpdateDeliveriesForDetail(
            int orderNumber, 
            int orderLine,
            IEnumerable<PurchaseOrderDeliveryResource> resource,
            IEnumerable<string> privileges)
        {
            try
            {
                var resourceList = resource.ToList();
                var entities = resourceList.Select(
                    d =>
                        {
                            if (!DateTime.TryParse(d.DateAdvised, out var dateAdvised) || !DateTime.TryParse(
                                    d.DateRequested,
                                    out var dateRequested))
                            {
                                throw new InvalidOperationException("Invalid Date supplied");
                            }

                            return new PurchaseOrderDelivery
                                       {
                                           DeliverySeq = d.DeliverySeq,
                                           OurDeliveryQty = d.OurDeliveryQty,
                                           Cancelled = d.Cancelled,
                                           DateAdvised = dateAdvised,
                                           DateRequested = dateRequested,
                                           NetTotalCurrency = d.NetTotalCurrency,
                                           BaseNetTotal = d.BaseNetTotal,
                                           OrderDeliveryQty = d.OrderDeliveryQty,
                                           OrderLine = d.OrderLine,
                                           OrderNumber = d.OrderNumber,
                                           QtyNetReceived = d.QtyNetReceived,
                                           QuantityOutstanding = d.QuantityOutstanding,
                                           CallOffDate =
                                               string.IsNullOrEmpty(d.CallOffDate)
                                                   ? null
                                                   : DateTime.Parse(d.CallOffDate),
                                           BaseOurUnitPrice = d.BaseOurUnitPrice,
                                           SupplierConfirmationComment = d.SupplierConfirmationComment,
                                           OurUnitPriceCurrency = d.OurUnitPriceCurrency,
                                           OrderUnitPriceCurrency = d.OrderUnitPriceCurrency,
                                           BaseOrderUnitPrice = d.BaseOrderUnitPrice,
                                           VatTotalCurrency = d.VatTotalCurrency,
                                           BaseVatTotal = d.BaseVatTotal,
                                           DeliveryTotalCurrency = d.DeliveryTotalCurrency,
                                           BaseDeliveryTotal = d.BaseDeliveryTotal,
                                           RescheduleReason = d.RescheduleReason,
                                           AvailableAtSupplier = d.AvailableAtSupplier,
                                           CallOffRef = d.CallOffRef,
                                           FilCancelled = d.FilCancelled,
                                           QtyPassedForPayment = d.QtyPassedForPayment
                                       };
                        }).ToList();

                this.domainService.UpdateDeliveriesForOrderLine(
                    orderNumber,
                    orderLine,
                    entities,
                    privileges);
                this.transactionManager.Commit();

                // update the mini order to keep its deliveries in sync
                this.domainService.UpdateMiniOrderDeliveries(entities);
                this.transactionManager.Commit();

                return new SuccessResult<IEnumerable<PurchaseOrderDeliveryResource>>(resourceList);
            }
            catch (Exception e)
            {
                return new BadRequestResult<IEnumerable<PurchaseOrderDeliveryResource>>(e.Message);
            }
        }

        public IResult<IEnumerable<PurchaseOrderDeliveryResource>> GetDeliveriesForDetail(int orderNumber, int orderLine)
        {
            var res = this.domainService.SearchDeliveries(null, orderNumber.ToString(), true, orderLine);

            return new SuccessResult<IEnumerable<PurchaseOrderDeliveryResource>>(
                res.Select(x => (PurchaseOrderDeliveryResource)this.resourceBuilder.Build(x, null)));
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
