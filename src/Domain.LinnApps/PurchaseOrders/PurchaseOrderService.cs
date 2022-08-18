﻿namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Configuration;
    using Linn.Common.Email;
    using Linn.Common.Pdf;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IAuthorisationService authService;

        private readonly IDatabaseService databaseService;

        private readonly IEmailService emailService;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<MiniOrder, int> miniOrderRepository;

        private readonly IRepository<Supplier, int> supplierRepository;

        private readonly IRepository<LinnDeliveryAddress, int> linnDeliveryAddressRepository;

        private readonly ICurrencyPack currencyPack;

        private readonly IPdfService pdfService;

        private readonly IPurchaseLedgerPack purchaseLedgerPack;

        private readonly IPurchaseOrdersPack purchaseOrdersPack;

        public PurchaseOrderService(
            IAuthorisationService authService,
            IPurchaseLedgerPack purchaseLedgerPack,
            IDatabaseService databaseService,
            IPdfService pdfService,
            IEmailService emailService,
            IRepository<Employee, int> employeeRepository,
            IRepository<MiniOrder, int> miniOrderRepository,
            IRepository<Supplier, int> supplierRepository,
            IRepository<LinnDeliveryAddress, int> linnDeliveryAddressRepository,
            IPurchaseOrdersPack purchaseOrdersPack,
            ICurrencyPack currencyPack)
        {
            this.authService = authService;
            this.purchaseLedgerPack = purchaseLedgerPack;
            this.databaseService = databaseService;
            this.pdfService = pdfService;
            this.emailService = emailService;
            this.employeeRepository = employeeRepository;
            this.miniOrderRepository = miniOrderRepository;
            this.purchaseOrdersPack = purchaseOrdersPack;
            this.supplierRepository = supplierRepository;
            this.linnDeliveryAddressRepository = linnDeliveryAddressRepository;
            this.currencyPack = currencyPack;
        }

        public void AllowOverbook(
            PurchaseOrder current,
            string allowOverBook,
            decimal? overbookQty,
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to allow overbooks on a purchase order");
            }

            current.Overbook = allowOverBook;
            current.OverbookQty = overbookQty;
        }

        public PurchaseOrder CancelOrder(PurchaseOrder order, int currentUserId, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to cancel purchase orders");
            }

            order.Cancelled = "Y";

            var currentLedgerPeriod = this.purchaseLedgerPack.GetLedgerPeriod();

            foreach (var detail in order.Details)
            {
                var cancelledDetail = new CancelledOrderDetail
                                          {
                                              Id = this.databaseService.GetNextVal("PLOC_SEQ"),
                                              OrderNumber = detail.OrderNumber,
                                              LineNumber = detail.Line,
                                              DateCancelled = DateTime.Today,
                                              PeriodCancelled = currentLedgerPeriod,
                                              CancelledById = currentUserId,
                                              ReasonCancelled = detail.Cancelled,
                                              ValueCancelled = detail.BaseDetailTotal

                                              // todo check for valuecancelled that:
                                              // baseDetailTotal == round(nvl(v_qty_outstanding, 0) * :new.base_our_price, 2)
                                          };
                detail.Cancelled = "Y";
                detail.CancelledDetails.Add(cancelledDetail);
            }

            return order;
        }

        public void CreateOrder(PurchaseOrder order, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderCreate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to create purchase orders");
            }

            // set conversion factor to 1 for now

            // add id to pl_order_postings using next val plorp_seq
            // add mini order for backwards compatibility for now?
        }

        public ProcessResult SendPdfEmail(
            string html,
            string emailAddress,
            int orderNumber,
            bool bcc,
            int currentUserId,
            PurchaseOrder order)
        {
            var pdf = this.pdfService.ConvertHtmlToPdf(html, false);
            var emailBody = $"Please accept the attached order no. {orderNumber}.\n"
                            + "You will need Acrobat Reader to open the file which is available from www.adobe.com/acrobat\n"
                            + "Linn's standard Terms & Conditions apply at all times\n"
                            + "and can be found at www.linn.co.uk/purchasing_conditions";

            var bccList = new List<Dictionary<string, string>>
                              {
                                  new Dictionary<string, string>
                                      {
                                          { "name", "purchasing outgoing" },
                                          { "address", ConfigurationManager.Configuration["PURCHASING_FROM_ADDRESS"] }
                                      }
                              };
            if (bcc)
            {
                var employee = this.employeeRepository.FindById(currentUserId);
                bccList.Add(
                    new Dictionary<string, string>
                        {
                            { "name", employee.FullName }, { "address", employee.PhoneListEntry?.EmailAddress }
                        });
            }

            this.emailService.SendEmail(
                    emailAddress,
                    emailAddress,
                    null,
                    bccList,
                    ConfigurationManager.Configuration["PURCHASING_FROM_ADDRESS"],
                    "Linn Purchasing",
                    $"Linn Purchase Order {orderNumber}",
                    emailBody,
                    "pdf",
                    pdf.Result,
                    $"LinnPurchaseOrder{orderNumber}");

            order.SentByMethod = "EMAIL";

            //// todo When get rid of mini orders remove below
            var miniOrder = this.miniOrderRepository.FindById(orderNumber);
            miniOrder.SentByMethod = "EMAIL";

            return new ProcessResult(true, $"Email sent for purchase order {orderNumber} to {emailAddress}");
        }

        public PurchaseOrder UpdateOrder(PurchaseOrder current, PurchaseOrder updated, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update purchase orders");
            }

            this.UpdateOrderProperties(current, updated);
            this.UpdateDetails(current.Details, updated.Details, updated.SupplierId, updated.ExchangeRate.Value);
            this.UpdateMiniOrder(updated);
            return current;
        }

        public PurchaseOrder FillOutUnsavedOrder(PurchaseOrder order, int currentUserId)
        {
            if (order.Supplier?.SupplierId == null)
            {
                throw new ArgumentNullException();
            }

            var supplier = this.supplierRepository.FindById(order.Supplier.SupplierId);

            order.Supplier = supplier;
            order.OrderAddress = supplier.OrderAddress;
            order.OrderAddressId = supplier.OrderAddress.AddressId;
            order.CurrencyCode = supplier.Currency.Code;
            order.Currency = supplier.Currency;
            order.OrderDate = DateTime.Now;

            if (order.DeliveryAddress == null)
            {
                var mainDeliveryAddress = this.linnDeliveryAddressRepository.FindAll().First(d => d.IsMainDeliveryAddress == "Y");
                order.DeliveryAddressId = mainDeliveryAddress.AddressId;
                order.DeliveryAddress = mainDeliveryAddress;
            }

            order.DocumentTypeName = "PO";
            order.DocumentType.Name = "PO";
            order.DocumentType.Description = "PURCHASE ORDER";
            order.OrderMethodName = "MANUAL";
            order.OrderMethod.Name = "MANUAL";
            order.OrderMethod.Description = "MANUAL ORDERING";

            var user = this.employeeRepository.FindById(currentUserId);
            order.RequestedById = currentUserId;
            order.RequestedBy = user;
            order.EnteredById = currentUserId;
            order.EnteredBy = user;

            order.ExchangeRate = this.currencyPack.GetExchangeRate("GBP", order.CurrencyCode);

            return order;
        }

        public ProcessResult AuthoriseMultiplePurchaseOrders(IEnumerable<int> orderNumbers, int userNumber)
        {
            return new ProcessResult(true, "ok \n Those authorised fine");
        }

        public ProcessResult EmailMultiplePurchaseOrders(IEnumerable<int> orderNumbers, int userNumber, bool copyToSelf)
        {
            return new ProcessResult(true, "ok \n Those emailed fine");
        }

        // below method currently unreferenced, but to be finished and used soon for create
        private void CreateMiniOrder(PurchaseOrder order)
        {
            var miniOrder = new MiniOrder();
            var detail = order.Details.First();

            miniOrder.OrderNumber = order.OrderNumber;
            miniOrder.DocumentType = order.DocumentType.Name;
            miniOrder.DateOfOrder = order.OrderDate;
            miniOrder.RequestedDeliveryDate = detail.PurchaseDeliveries.First().DateRequested;
            miniOrder.AdvisedDeliveryDate = detail.PurchaseDeliveries.First().DateAdvised;
            miniOrder.Remarks = order.Remarks;
            miniOrder.SupplierId = order.SupplierId;
            miniOrder.PartNumber = detail.PartNumber;
            miniOrder.Currency = order.Currency.Code;
            miniOrder.SuppliersDesignation = detail.SuppliersDesignation;
            miniOrder.Department = detail.OrderPosting.NominalAccount.Department.DepartmentCode;
            miniOrder.Nominal = detail.OrderPosting.NominalAccount.Nominal.NominalCode;
            miniOrder.AuthorisedBy = order.AuthorisedBy.Id;
            miniOrder.EnteredBy = order.EnteredBy.Id;
            miniOrder.OurUnitOfMeasure = detail.OurUnitOfMeasure;
            miniOrder.OrderUnitOfMeasure = detail.OrderUnitOfMeasure;
            miniOrder.RequestedBy = order.RequestedById;
            miniOrder.DeliveryInstructions = detail.DeliveryInstructions;
            miniOrder.OurQty = detail.OurQty;
            miniOrder.OrderQty = detail.OrderQty;
            miniOrder.OrderConvFactor = detail.OrderConversionFactor;
            miniOrder.NetTotal = detail.NetTotalCurrency;
            miniOrder.VatTotal = detail.VatTotalCurrency.GetValueOrDefault(0);
            miniOrder.OrderTotal = detail.DetailTotalCurrency.GetValueOrDefault(0);
            miniOrder.OrderMethod = order.OrderMethod.Name;
            miniOrder.CancelledBy = detail.CancelledDetails.First().CancelledById;
            miniOrder.ReasonCancelled = detail.CancelledDetails.First().ReasonCancelled;
            miniOrder.SentByMethod = order.SentByMethod;
            miniOrder.AcknowledgeComment = detail.PurchaseDeliveries.First().SupplierConfirmationComment;
            miniOrder.DeliveryAddressId = order.DeliveryAddressId;
            miniOrder.NumberOfSplitDeliveries = detail.PurchaseDeliveries.Count;
            miniOrder.QuotationRef = order.QuotationRef;
            miniOrder.IssuePartsToSupplier = order.IssuePartsToSupplier;
            miniOrder.Vehicle = detail.OrderPosting.Vehicle;
            miniOrder.Building = detail.OrderPosting.Building;
            miniOrder.Product = detail.OrderPosting.Product;
            miniOrder.Person = detail.OrderPosting.Person;

            // miniOrder.DrawingReference = detail.dr; //dont think needed
            miniOrder.StockPoolCode = detail.StockPoolCode;

            // miniOrder.PrevOrderNumber = detail.;
            // miniOrder.PrevOrderLine = updatedOrder.;
            miniOrder.FilCancelledBy = detail.CancelledDetails.FirstOrDefault()?.FilCancelledById;
            miniOrder.ReasonFilCancelled = detail.CancelledDetails.FirstOrDefault()?.ReasonCancelled;
            miniOrder.OurPrice = detail.OurUnitPriceCurrency;
            miniOrder.OrderPrice = detail.OrderUnitPriceCurrency;
            miniOrder.BaseCurrency = order.BaseCurrencyCode;
            miniOrder.BaseOurPrice = detail.BaseOurUnitPrice;
            miniOrder.BaseOrderPrice = detail.BaseOrderUnitPrice;
            miniOrder.BaseNetTotal = detail.BaseNetTotal;
            miniOrder.BaseVatTotal = detail.BaseVatTotal;
            miniOrder.BaseOrderTotal = detail.BaseDetailTotal;
            miniOrder.ExchangeRate = order.ExchangeRate;

            // miniOrder.ManufacturerPartNumber = updatedOrder.;
            miniOrder.DateFilCancelled = detail.CancelledDetails.First().DateFilCancelled;
            miniOrder.RohsCompliant = detail.RohsCompliant;

            // miniOrder.ShouldHaveBeenBlueReq = updatedOrder.;
            // miniOrder.SpecialOrderType = updatedOrder.;
            // miniOrder.PpvAuthorisedBy = updatedOrder.;
            // miniOrder.PpvReason = updatedOrder.;
            // miniOrder.MpvAuthorisedBy = updatedOrder.
            // miniOrder.MpvReason = updatedOrder.
            miniOrder.DeliveryConfirmedBy = detail.DeliveryConfirmedBy.Id;

            // miniOrder.TotalQtyDelivered = updatedOrder.Details
            miniOrder.InternalComments = detail.InternalComments;

            this.miniOrderRepository.Add(miniOrder);
        }

        private void UpdateDeliveries(
            ICollection<PurchaseOrderDelivery> deliveries,
            ICollection<PurchaseOrderDelivery> updatedDeliveries)
        {
            foreach (var delivery in deliveries)
            {
                var updatedDelivery = updatedDeliveries.First(x => x.DeliverySeq == delivery.DeliverySeq);
                delivery.DateRequested = updatedDelivery.DateRequested;

                // price updates to be done next
                // delivery.OurUnitPriceCurrency = updatedDelivery.OurUnitPriceCurrency;
                // delivery.OrderUnitPriceCurrency = updatedDelivery.OrderUnitPriceCurrency;
                // delivery.BaseOrderUnitPrice = updatedDelivery.BaseOrderUnitPrice;
                // delivery.BaseOurUnitPrice = updatedDelivery.BaseOurUnitPrice;
                // delivery.VatTotalCurrency = updatedDelivery.VatTotalCurrency;
                // delivery.BaseVatTotal = updatedDelivery.BaseVatTotal; //// vat totals might not change
                // delivery.DeliveryTotalCurrency = updatedDelivery.DeliveryTotalCurrency;
                // delivery.BaseDetailTotal = updatedDelivery.BaseDetailTotal;
            }
        }

        private void UpdateDetailProperties(
            PurchaseOrderDetail current,
            PurchaseOrderDetail updated,
            int supplierId,
            decimal exchangeRate)
        {
            current.SuppliersDesignation = updated.SuppliersDesignation;
            current.InternalComments = updated.InternalComments;

            var netTotal = updated.OurUnitPriceCurrency.GetValueOrDefault() * updated.OurQty.GetValueOrDefault();
            current.NetTotalCurrency = Math.Round(netTotal, 2, MidpointRounding.AwayFromZero);

            current.VatTotalCurrency =
                this.purchaseOrdersPack.GetVatAmountSupplier(current.NetTotalCurrency, supplierId);

            current.DetailTotalCurrency = Math.Round(netTotal + current.VatTotalCurrency.Value, 2);

            if (updated.OrderUnitPriceCurrency == current.OrderUnitPriceCurrency
                && updated.OurUnitPriceCurrency != current.OurUnitPriceCurrency)
            {
                // if order price hasn't been overridden but our price has changed, use conv factor
                current.OrderUnitPriceCurrency = updated.OurUnitPriceCurrency * current.OrderConversionFactor;
            }
            else if (updated.OrderUnitPriceCurrency != current.OrderUnitPriceCurrency)
            {
                // if order price has been manually overridden
                current.OrderUnitPriceCurrency = updated.OrderUnitPriceCurrency;
            }

            if (updated.OrderQty == current.OrderQty && updated.OurQty != current.OurQty)
            {
                // if order qty hasn't been overridden but our qty has changed, use conv factor
                current.OrderQty = updated.OurQty * current.OrderConversionFactor;
            }
            else if (updated.OrderQty != current.OrderQty)
            {
                // if our qty has been manually overridden
                current.OrderQty = updated.OrderQty;
            }

            current.OurQty = updated.OurQty;
            current.OurUnitPriceCurrency = updated.OurUnitPriceCurrency;

            current.BaseNetTotal = Math.Round(netTotal / exchangeRate, 2, MidpointRounding.AwayFromZero);
            current.BaseOrderUnitPrice = Math.Round(
                current.OrderUnitPriceCurrency.GetValueOrDefault() / exchangeRate,
                2,
                MidpointRounding.AwayFromZero);
            current.BaseOurUnitPrice = Math.Round(
                current.OurUnitPriceCurrency.GetValueOrDefault() / exchangeRate,
                2,
                MidpointRounding.AwayFromZero);

            current.BaseVatTotal = Math.Round(
                current.VatTotalCurrency.GetValueOrDefault() / exchangeRate,
                2,
                MidpointRounding.AwayFromZero);

            current.BaseDetailTotal = Math.Round(
                current.DetailTotalCurrency.GetValueOrDefault() / exchangeRate,
                2,
                MidpointRounding.AwayFromZero);

            this.UpdateOrderPostingsForDetail(current, updated);

            this.UpdateDeliveries(current.PurchaseDeliveries, updated.PurchaseDeliveries);
        }

        private void UpdateDetails(
            ICollection<PurchaseOrderDetail> currentDetails,
            ICollection<PurchaseOrderDetail> updatedDetails,
            int supplierId,
            decimal exchangeRate)
        {
            foreach (var detail in updatedDetails)
            {
                var currentDetail = currentDetails.FirstOrDefault(x => x.Line == detail.Line);
                if (currentDetail != null)
                {
                    this.UpdateDetailProperties(currentDetail, detail, supplierId, exchangeRate);
                }
                else
                {
                    currentDetails.Add(detail);
                }
            }
        }

        private void UpdateMiniOrder(PurchaseOrder updatedOrder)
        {
            var miniOrder = this.miniOrderRepository.FindById(updatedOrder.OrderNumber);
            var updatedDetail = updatedOrder.Details.First();

            miniOrder.Remarks = updatedOrder.Remarks;
            miniOrder.Department = updatedDetail.OrderPosting.NominalAccount.Department.DepartmentCode;
            miniOrder.Nominal = updatedDetail.OrderPosting.NominalAccount.Nominal.NominalCode;
            miniOrder.RequestedDeliveryDate = updatedDetail.PurchaseDeliveries.First().DateRequested;
            miniOrder.InternalComments = updatedDetail.InternalComments;
            miniOrder.SuppliersDesignation = updatedDetail.SuppliersDesignation;

            var netTotal = updatedDetail.OurUnitPriceCurrency.GetValueOrDefault()
                           * updatedDetail.OurQty.GetValueOrDefault();
            miniOrder.NetTotal = Math.Round(netTotal, 2, MidpointRounding.AwayFromZero);

            miniOrder.VatTotal =
                this.purchaseOrdersPack.GetVatAmountSupplier(miniOrder.NetTotal, updatedOrder.SupplierId);

            miniOrder.OrderTotal = Math.Round(netTotal + miniOrder.VatTotal, 2);

            if (updatedDetail.OrderUnitPriceCurrency == miniOrder.OrderPrice
                && updatedDetail.OurUnitPriceCurrency != miniOrder.OurPrice)
            {
                // if order price hasn't been overridden but our price has changed, use conv factor
                miniOrder.OrderPrice = updatedDetail.OurUnitPriceCurrency * miniOrder.OrderConvFactor;
            }
            else if (updatedDetail.OrderUnitPriceCurrency != miniOrder.OrderPrice)
            {
                // if order price has been manually overridden
                miniOrder.OrderPrice = updatedDetail.OrderUnitPriceCurrency;
            }

            if (updatedDetail.OrderQty == miniOrder.OrderQty && updatedDetail.OurQty != miniOrder.OurQty)
            {
                // if order qty hasn't been overridden but our qty has changed, use conv factor
                miniOrder.OrderQty = updatedDetail.OurQty * miniOrder.OrderConvFactor;
            }
            else if (updatedDetail.OrderQty != miniOrder.OrderQty)
            {
                // if our qty has been manually overridden
                miniOrder.OrderQty = updatedDetail.OrderQty;
            }

            miniOrder.OurQty = updatedDetail.OurQty;
            miniOrder.OurPrice = updatedDetail.OurUnitPriceCurrency;

            var exchangeRate = updatedOrder.ExchangeRate.GetValueOrDefault();

            miniOrder.BaseNetTotal = Math.Round(netTotal / exchangeRate, 2, MidpointRounding.AwayFromZero);
            miniOrder.BaseOrderPrice = Math.Round(
                miniOrder.OrderPrice.GetValueOrDefault() / exchangeRate,
                2,
                MidpointRounding.AwayFromZero);
            miniOrder.BaseOurPrice = Math.Round(
                miniOrder.OurPrice.GetValueOrDefault() / exchangeRate,
                2,
                MidpointRounding.AwayFromZero);

            miniOrder.BaseVatTotal = Math.Round(miniOrder.VatTotal / exchangeRate, 2, MidpointRounding.AwayFromZero);

            miniOrder.BaseOrderTotal = Math.Round(
                miniOrder.OrderTotal / exchangeRate,
                2,
                MidpointRounding.AwayFromZero);
        }

        private void UpdateOrderPostingsForDetail(PurchaseOrderDetail current, PurchaseOrderDetail updated)
        {
            if (current.OrderPosting.NominalAccountId != updated.OrderPosting.NominalAccountId)
            {
                current.OrderPosting.NominalAccountId = updated.OrderPosting.NominalAccountId;
            }
        }

        private void UpdateOrderProperties(PurchaseOrder current, PurchaseOrder updated)
        {
            current.Remarks = updated.Remarks;
        }
    }
}
