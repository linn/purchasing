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

        public IResult<BatchUpdateProcessResultResource> BatchUpdateDeliveries(
            string csvString, IEnumerable<string> privileges)
        {
            var reader = new StringReader(csvString);
            var changes = new List<PurchaseOrderDeliveryUpdate>();
            try
            {
                while (reader.ReadLine() is { } line)
                {
                    // assuming csv lines are in the form <orderNumber>,<deliveryNo>,<newAdvisedDate>,<qty>, <unitPrice>, <newReason>
                    var row = line.Split(",");

                    if (!int.TryParse(
                            new string(row[0].Trim().Where(char.IsDigit).ToArray()), // strip out non numeric chars 
                            out var orderNumber))
                    {
                        throw new InvalidOperationException($"Invalid Order Number: {row[0]}.");
                    }

                    if (!int.TryParse(row[1].Trim(), out var delNo))
                    {
                        throw new InvalidOperationException($"Invalid Delivery Number: {row[0]} / {row[1]}.");
                    }

                    if (!int.TryParse(row[3].Trim(), out var qty))
                    {
                        throw new InvalidOperationException($"Invalid Qty for {row[0]} / {row[1]}.");
                    }

                    if (!decimal.TryParse(row[4].Trim(), out var unitPrice))
                    {
                        throw new InvalidOperationException($"Invalid Unit Price for {row[0]} / {row[1]}.");
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
                                        NewDateAdvised = firstFormatSatisfied ? parsedDate1 : parsedDate2,
                                        NewReason = row.Length < 5 ? null : row[5].Trim(),
                                        Qty = qty,
                                        UnitPrice = unitPrice
                                    });
                }

                var result = this.domainService.BatchUpdateDeliveries(changes, privileges, true);
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

        public IResult<BatchUpdateProcessResultResource> BatchUpdateDeliveries(
            IEnumerable<PurchaseOrderDeliveryUpdateResource> resource, IEnumerable<string> privileges)
        {
            var updates = resource.Select(
                u => new PurchaseOrderDeliveryUpdate
                         {
                             Key = new PurchaseOrderDeliveryKey
                                       {
                                           OrderNumber = u.OrderNumber,
                                           OrderLine = u.OrderLine,
                                           DeliverySequence = u.DeliverySequence
                                       },
                             NewDateAdvised = u.DateAdvised,
                             NewReason = u.Reason,
                             Qty = u.Qty,
                             AvailableAtSupplier = u.AvailableAtSupplier,
                             Comment = u.Comment
                         }).ToList();
            var result = this.domainService
                .BatchUpdateDeliveries(updates, privileges);

            this.transactionManager.Commit();

            // update the mini order to keep its deliveries in sync
            this.domainService.UpdateMiniOrderDeliveries(updates.Select(
                u => new PurchaseOrderDelivery
                         {
                            OrderNumber = u.Key.OrderNumber,
                            DeliverySeq = u.Key.DeliverySequence,
                            OrderLine = u.Key.OrderLine,
                            DateAdvised = u.NewDateAdvised,
                            AvailableAtSupplier = u.AvailableAtSupplier,
                            SupplierConfirmationComment = u.Comment,
                            RescheduleReason = u.NewReason,
                            OurDeliveryQty = u.Qty
                         }));

            this.transactionManager.Commit();

            return new SuccessResult<BatchUpdateProcessResultResource>(new BatchUpdateProcessResultResource
                                                                           {
                                                                               Message = result.Message,
                                                                               Success = result.Success,
                                                                               Errors = result.Errors?.Select(e => new ErrorResource
                                                                                   {
                                                                                       Descriptor = e.Descriptor,
                                                                                       Message = e.Message
                                                                                   })
                                                                           });
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
                            var dateAdvised = new DateTime();

                            if (!string.IsNullOrEmpty(d.DateAdvised) && (!DateTime.TryParse(d.DateAdvised, out dateAdvised)) 
                                || !DateTime.TryParse(
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
                                           DateAdvised = string.IsNullOrEmpty(d.DateAdvised) ? null : dateAdvised,
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
    }
}
