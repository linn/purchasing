namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Configuration;
    using Linn.Common.Email;
    using Linn.Common.Logging;
    using Linn.Common.Pdf;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Common.Rendering;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Finance.Models;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Org.BouncyCastle.Asn1.X509;

    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IAuthorisationService authService;

        private readonly IDatabaseService databaseService;

        private readonly IEmailService emailService;

        private readonly ISupplierKitService supplierKitService;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<MiniOrder, int> miniOrderRepository;

        private readonly IRepository<Supplier, int> supplierRepository;

        private readonly IRepository<LinnDeliveryAddress, int> linnDeliveryAddressRepository;

        private readonly ICurrencyPack currencyPack;

        private readonly IRepository<PurchaseOrder, int> purchaseOrderRepository;

        private readonly IHtmlTemplateService<PurchaseOrder> purchaseOrderTemplateService;

        private readonly ILog log;

        private readonly IPdfService pdfService;

        private readonly IPurchaseLedgerPack purchaseLedgerPack;

        private readonly IPurchaseOrdersPack purchaseOrdersPack;

        private readonly ISingleRecordRepository<PurchaseLedgerMaster> purchaseLedgerMaster;

        private readonly IRepository<NominalAccount, int> nominalAccountRepository;

        private readonly IQueryRepository<Part> partQueryRepository;

        private readonly IRepository<PartSupplier, PartSupplierKey> partSupplierRepository;

        private readonly IHtmlTemplateService<PlCreditDebitNote> creditDebitNoteHtmlService;

        private readonly IRepository<PlCreditDebitNote, int> creditDebitNoteRepository;

        private readonly IQueryRepository<PlOrderReceivedViewEntry> orderReceivedView;

        private readonly IRepository<CancelledOrderDetail, int> cancelledOrderDetailRepository;

        private readonly IQueryRepository<ImmediateLiability> liabilityRepository;

        private readonly IQueryRepository<ImmediateLiabilityBase> baseLiabilityRepository;

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
            ICurrencyPack currencyPack,
            ISupplierKitService supplierKitService,
            IRepository<PurchaseOrder, int> purchaseOrderRepository,
            IHtmlTemplateService<PurchaseOrder> purchaseOrderTemplateService,
            ISingleRecordRepository<PurchaseLedgerMaster> purchaseLedgerMaster,
            IRepository<NominalAccount, int> nominalAccountRepository,
            IQueryRepository<Part> partQueryRepository,
            IRepository<PartSupplier, PartSupplierKey> partSupplierRepository,
            IHtmlTemplateService<PlCreditDebitNote> creditDebitNoteHtmlService,
            ILog log,
            IRepository<PlCreditDebitNote, int> creditDebitNoteRepository,
            IQueryRepository<PlOrderReceivedViewEntry> orderReceivedView,
            IRepository<CancelledOrderDetail, int> cancelledOrderDetailRepository,
            IQueryRepository<ImmediateLiability> liabilityRepository,
            IQueryRepository<ImmediateLiabilityBase> baseLiabilityRepository)
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
            this.supplierKitService = supplierKitService;
            this.purchaseOrderRepository = purchaseOrderRepository;
            this.purchaseOrderTemplateService = purchaseOrderTemplateService;
            this.purchaseLedgerMaster = purchaseLedgerMaster;
            this.nominalAccountRepository = nominalAccountRepository;
            this.partQueryRepository = partQueryRepository;
            this.partSupplierRepository = partSupplierRepository;
            this.log = log;
            this.creditDebitNoteHtmlService = creditDebitNoteHtmlService;
            this.creditDebitNoteRepository = creditDebitNoteRepository;
            this.orderReceivedView = orderReceivedView;
            this.cancelledOrderDetailRepository = cancelledOrderDetailRepository;
            this.liabilityRepository = liabilityRepository;
            this.baseLiabilityRepository = baseLiabilityRepository;
        }

        public PurchaseOrder AllowOverbook(
            PurchaseOrder current,
            string allowOverBook,
            decimal? overbookQty,
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to allow overbooks on a purchase order");
            }

            this.CheckOkToRaiseOrders();

            current.Overbook = allowOverBook;
            current.OverbookQty = overbookQty;

            return current;
        }

        public PurchaseOrder CancelOrder(int orderNumber, int cancelledBy, string reason, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderCancel, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to cancel purchase orders");
            }

            var order = this.GetOrder(orderNumber);

            order.Cancelled = "Y";

            var currentLedgerPeriod = this.purchaseLedgerPack.GetLedgerPeriod();

            foreach (var detail in order.Details)
            {
                var id = this.databaseService.GetIdSequence("PLOC_SEQ");
                var cancelledDetail = new CancelledOrderDetail
                                          {
                                              Id = id,
                                              OrderNumber = detail.OrderNumber,
                                              LineNumber = detail.Line,
                                              DateCancelled = DateTime.Today,
                                              PeriodCancelled = currentLedgerPeriod,
                                              CancelledById = cancelledBy,
                                              ReasonCancelled = reason,
                                              ValueCancelled = Math.Round(
                                                  detail.BaseOurUnitPrice.GetValueOrDefault() *
                                                  this.orderReceivedView.FindBy(
                                                      x => x.OrderNumber == orderNumber && x.OrderLine == detail.Line)
                                                      .QtyOutstanding, 2)
                                          };
                detail.Cancelled = "Y";
                this.cancelledOrderDetailRepository.Add(cancelledDetail);
            }

            var miniOrder = this.miniOrderRepository.FindById(orderNumber);
            miniOrder.CancelledBy = cancelledBy;
            miniOrder.ReasonCancelled = reason;

            return order;
        }

        public PurchaseOrder CreateOrder(PurchaseOrder order, IEnumerable<string> privileges, out bool createCreditNote)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderCreate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to create purchase orders");
            }

            this.CheckOkToRaiseOrders();

            if (order.Details.Any(d => !d.ValidPrices()))
            {
                throw new PurchaseOrderException("Prices must be positive numbers");
            }

            var newOrderNumber = this.databaseService.GetNextVal("PL_ORDER_SEQ");
            order.Supplier = this.supplierRepository.FindById(order.SupplierId);
            order.OrderNumber = newOrderNumber;
            order.OrderNetTotal = 0;
            order.BaseOrderNetTotal = 0;
            order.OrderVatTotal = 0;
            order.BaseOrderVatTotal = 0;
            order.OrderTotal = 0;
            order.BaseOrderTotal = 0;
          
            foreach (var detail in order.Details)
            {
                var part = this.partQueryRepository.FindBy(x => x.PartNumber == detail.PartNumber);

                if (part == null)
                {
                    throw new PurchaseOrderException($"Cannot order part {detail.PartNumber} as it could not be found");
                }
                
                if (part.DatePurchPhasedOut.HasValue)
                {
                    throw new PurchaseOrderException($"Cannot order part {detail.PartNumber} as it has been phased out");
                }

                if (!part.DateLive.HasValue)
                {
                    throw new PurchaseOrderException("Cannot Order Non-Live Part!");
                }

                var nominal = detail.OrderPosting.NominalAccount;

                var nominalAccounts = this.nominalAccountRepository.FilterBy(x => x.NominalCode == nominal.NominalCode);

                if (!nominalAccounts.Any())
                {
                    nominalAccounts = this.nominalAccountRepository.FilterBy(
                        x => x.NominalCode.EndsWith(nominal.NominalCode));
                }

                var nominalAccount = nominalAccounts.FirstOrDefault(
                                         x => x.DepartmentCode == nominal.DepartmentCode)
                                     ?? nominalAccounts.FirstOrDefault(
                                         x => x.DepartmentCode.EndsWith(nominal.DepartmentCode));

                if (nominalAccount == null)
                {
                    throw new ItemNotFoundException("Invalid nominal code/dept");
                }
                
                detail.OrderPosting.NominalAccount = nominalAccount;

                this.SetDetailFieldsForCreation(detail, newOrderNumber);

                this.PerformDetailCalculations(
                    detail, detail, order.ExchangeRate.GetValueOrDefault(1), order.SupplierId, creating: true);

                this.AddDeliveryToDetail(order, detail);

                order.OrderNetTotal += detail.NetTotalCurrency;
                order.BaseOrderNetTotal += detail.BaseNetTotal;
                order.OrderVatTotal += detail.VatTotalCurrency.GetValueOrDefault();
                order.BaseOrderVatTotal += detail.BaseVatTotal.GetValueOrDefault();
                order.OrderTotal += detail.DetailTotalCurrency.GetValueOrDefault();
                order.BaseOrderTotal += detail.BaseDetailTotal.GetValueOrDefault();
            }

            order.Cancelled = "N";
            order.FilCancelled = "N";
            order.ArchiveOrder = "N";
            order.BaseCurrencyCode = "GBP";
            order.DamagesPercent = 2m;

            this.purchaseOrderRepository.Add(order);

            if (order.DocumentTypeName is "CO" or "RO")
            {
                createCreditNote = true;
            }
            else
            {
                createCreditNote = false;
            }

            return order;
        }

        public ProcessResult SendPdfEmail(string emailAddress, int orderNumber, bool bcc, int currentUserId)
        {
            var order = this.GetOrder(orderNumber);
            if (order.AuthorisedById is null)
            {
                throw new UnauthorisedOrderException("You cannot email an order until it has been authorised");
            }

            string debitNoteHtml = null;
            string debitNoteMessage = string.Empty;

            if (order.DocumentType.Name is "RO" or "CO")
            {
                var debitNote = this.creditDebitNoteRepository.FindBy(x => x.ReturnsOrderNumber == orderNumber);
                if (debitNote != null)
                {
                    debitNoteHtml = this.creditDebitNoteHtmlService.GetHtml(debitNote).Result;
                    debitNoteMessage = $"and debit note {debitNote.NoteNumber} ";
                }
            }

            var html = this.purchaseOrderTemplateService.GetHtml(order).Result;

            var ccList = order.Supplier?.SupplierContacts?.FirstOrDefault(x => x.IsMainOrderContact == "Y")
                ?.OrderCcAddresses;

            this.SendOrderPdfEmail(html, emailAddress, bcc, currentUserId, order, debitNoteHtml, ccList);

            return new ProcessResult(true, $"Email sent for purchase order {orderNumber} {debitNoteMessage}to {emailAddress}");
        }

        public ProcessResult SendSupplierAssemblyEmail(int orderNumber)
        {
            var order = this.GetOrder(orderNumber);

            var emailBody = $"Purchasing have raised order {orderNumber} for {order.Supplier.Name}.\n" 
                                + "This email is sent from an unmonitored account. Please send all correspondence direct to the buyer.\n"
                                + $"The following parts will need supplier kits\n";

            var kits = this.supplierKitService.GetSupplierKits(order, true);

            foreach (var kit in kits)
            {
                emailBody += "\n" + $"{kit.Qty} x {kit.Part.PartNumber} {kit.Part.Description}"
                             + "\n requires";
                foreach (var detail in kit.Details)
                {
                    emailBody += "\n    " + $"{detail.Qty} x {detail.Part.PartNumber} {detail.Part.Description}";
                }

                emailBody += "\n";
            }

            this.emailService.SendEmail(
                ConfigurationManager.Configuration["LOGISTICS_TO_ADDRESS"],
                "Logistics",
                null,
                null,
                ConfigurationManager.Configuration["PURCHASING_FROM_ADDRESS"],
                "Linn Purchasing",
                $"Purchase Order {orderNumber}",
                emailBody);

            this.log.Write(
                LoggingLevel.Info,
                new List<LoggingProperty>(),
                $"Email sent for purchase order {orderNumber} to Logistics");

            return new ProcessResult(true, $"Email sent for purchase order {orderNumber} to Logistics");
        }

        public ProcessResult SendFinanceAuthRequestEmail(int currentUserId, int orderNumber)
        {
            var order = this.GetOrder(orderNumber);

            var user = this.employeeRepository.FindById(currentUserId);
            var orderUrl = $"{ConfigurationManager.Configuration["APP_ROOT"]}/purchasing/purchase-orders/{orderNumber}";
            var emailBody = $"Purchasing have raised order {orderNumber} for {order.Supplier.Name}.\n"
                            + $"{user.FullName} would like you to Authorise it which you can do here:\n"
                            + $"{orderUrl} \n"
                             + $"Thanks";

            var employee1 = this.employeeRepository.FindById(32864);
            var employee2 = this.employeeRepository.FindById(32835);

            var cc = new List<Dictionary<string, string>>
                              {
                                  new Dictionary<string, string>
                                      {
                                          { "name", employee2.FullName },
                                          { "address", employee2.PhoneListEntry.EmailAddress }
                                      }
                              };
            var bcc = new List<Dictionary<string, string>>
                         {
                             new Dictionary<string, string>
                                 {
                                     { "name", user.FullName },
                                     { "address", user.PhoneListEntry.EmailAddress }
                                 }
                         };

            this.emailService.SendEmail(
                employee1.PhoneListEntry.EmailAddress,
                employee1.FullName,
                cc,
                bcc,
                ConfigurationManager.Configuration["PURCHASING_FROM_ADDRESS"],
                "Linn Purchasing",
                $"Purchase Order {orderNumber} requires Authorisation",
                emailBody);

            this.log.Write(
                LoggingLevel.Info,
                new List<LoggingProperty>(),
                $"Email sent for purchase order {orderNumber} auth request to Finance");

            return new ProcessResult(true, $"Email sent for purchase order {orderNumber} auth request to Finance");
        }

        public PurchaseOrder UpdateOrder(PurchaseOrder current, PurchaseOrder updated, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update purchase orders");
            }

            if (updated.Details.Any(d => !d.ValidPrices()))
            {
                throw new PurchaseOrderException("Prices must be positive numbers");
            }

            this.CheckOkToRaiseOrders();
            this.UpdateOrderProperties(current, updated);
            this.UpdateDetails(current.Details, updated.Details, updated.SupplierId, updated.ExchangeRate.Value, current.DocumentType.Name);
            this.UpdateMiniOrder(current);
            return current;
        }

        public PurchaseOrder FillOutUnsavedOrder(PurchaseOrder order, int currentUserId)
        {
            if (order.Supplier?.SupplierId == null)
            {
                throw new PurchaseOrderException("Cannot create a purchase order without a supplier");
            }

            var supplier = this.supplierRepository.FindById(order.Supplier.SupplierId);

            if (supplier.DateClosed != null)
            {
                throw new PurchaseOrderException("Supplier is closed!");
            }

            order.Supplier = supplier;
            order.OrderAddress = supplier.OrderAddress;
            order.OrderAddressId = supplier.OrderAddress.AddressId;
            order.InvoiceAddressId = supplier.InvoiceFullAddress.Id;

            order.CurrencyCode = supplier.Currency.Code;
            order.Currency = supplier.Currency;
            order.OrderDate = DateTime.Now;

            if (order.DeliveryAddress == null)
            {
                var mainDeliveryAddress = this.linnDeliveryAddressRepository.FindAll().First(d => d.IsMainDeliveryAddress == "Y");
                order.DeliveryAddressId = mainDeliveryAddress.AddressId;
                order.DeliveryAddress = mainDeliveryAddress;
            }

            order.DocumentTypeName = string.IsNullOrEmpty(order.DocumentTypeName) ? "PO" : order.DocumentTypeName;
            var documentTypeDescription = order.DocumentTypeName switch
                {
                    "PO" => "PURCHASE ORDER",
                    "RO" => "RETURNS ORDER",
                    "CO" => "CREDIT ORDER",
                    _ => string.Empty
                };
            order.DocumentType = new DocumentType
                                     {
                                         Name = order.DocumentTypeName,
                                         Description = documentTypeDescription
                                     };
            order.OrderMethodName = "MANUAL";
            order.OrderMethod = new OrderMethod { Name = "MANUAL", Description = "MANUAL ORDERING" };
            var user = this.employeeRepository.FindById(currentUserId);
            order.RequestedById = currentUserId;
            order.RequestedBy = user;
            order.EnteredById = currentUserId;
            order.EnteredBy = user;

            order.ExchangeRate = this.currencyPack.GetExchangeRate("GBP", order.CurrencyCode);

            if (order.ExchangeRate.GetValueOrDefault() == 0)
            {
                order.ExchangeRate = 1;
            }

            var detail = order.Details.First();
            detail.OrderUnitPriceCurrency = detail.OurUnitPriceCurrency;

            detail.OrderQty = detail.OurQty;

            var part = this.partQueryRepository.FindBy(p => p.PartNumber == detail.PartNumber);

            detail.OurUnitOfMeasure = part.OurUnitOfMeasure;
            detail.DrawingReference = part.DrawingReference;
            order.IssuePartsToSupplier = part.SupplierAssembly() ? "Y" : "N";

            var partSupplier = this.partSupplierRepository.FindById(new PartSupplierKey { PartNumber = detail.PartNumber, SupplierId = order.SupplierId });
            detail.OrderUnitOfMeasure = partSupplier != null ? partSupplier.UnitOfMeasure : part.OurUnitOfMeasure;
            detail.OurUnitOfMeasure = partSupplier != null ? partSupplier.UnitOfMeasure : part.OurUnitOfMeasure;
            detail.SuppliersDesignation = partSupplier != null ? partSupplier.SupplierDesignation : string.Empty;

            detail.NetTotalCurrency = detail.OurQty.GetValueOrDefault() 
                                      * detail.OurUnitPriceCurrency.GetValueOrDefault();

            detail.DetailTotalCurrency = detail.NetTotalCurrency + detail.VatTotalCurrency;
            detail.BaseNetTotal = Math.Round(detail.NetTotalCurrency / (decimal)order.ExchangeRate, 2);
            detail.BaseDetailTotal = 
                Math.Round(detail.DetailTotalCurrency.GetValueOrDefault() / (decimal)order.ExchangeRate, 2);

            NominalAccount nomAcc = null;
            if (part.StockControlled == "Y")
            {
                if (part.RawOrFinished == "R")
                {
                    nomAcc = this.nominalAccountRepository.FindBy(
                        a => a.NominalCode == "0000007617" && a.DepartmentCode == "0000002508");
                }
                else
                {
                    nomAcc = this.nominalAccountRepository.FindBy(
                        a => a.NominalCode == "0000007635" && a.DepartmentCode == "0000002508");
                }
            }
            else if (part.PartNumber != "SUNDRY")
            {
                nomAcc = part.NominalAccount;
            }
            
            detail.OrderPosting = nomAcc == null ?
                                      new PurchaseOrderPosting() 
                                      : new PurchaseOrderPosting
                                            {
                                                NominalAccount = nomAcc, 
                                                NominalAccountId = nomAcc.AccountId,
                                            };

            foreach (var d in order.Details)
            {
                var partSupplierRecord = this.partSupplierRepository.FindById(
                    new PartSupplierKey { SupplierId = order.SupplierId, PartNumber = d.PartNumber });

                if (this.partQueryRepository.FindBy(p => p.PartNumber == d.PartNumber).StockControlled == "Y" 
                    && partSupplierRecord != null
                    && d.PurchaseDeliveries?.First()?.DateRequested == null) // might already have a suggested date from MR
                {
                    d.PurchaseDeliveries = new List<PurchaseOrderDelivery>();

                    var deliveryDay = supplier.DeliveryDay ?? "MONDAY";

                    var leadTimeFromNow = DateTime.Today.AddDays(partSupplierRecord.LeadTimeWeeks * 7);
                    var dateRequested = NextOccurrenceOfDay(leadTimeFromNow, deliveryDay);

                    d.PurchaseDeliveries.Add(
                        new PurchaseOrderDelivery
                            {
                                DateRequested = dateRequested,
                                PurchaseOrderDetail = d
                            });
                }
            }

            return order;
        }

        public string GetOrderNotesForBuyer(PurchaseOrder order)
        {
            var notes = $"{order.Supplier?.NotesForBuyer} {Environment.NewLine}";
            foreach (var purchaseOrderDetail in order.Details)
            {
                var partSupplier = this.partSupplierRepository.FindById(
                    new PartSupplierKey { PartNumber = purchaseOrderDetail.PartNumber, SupplierId = order.SupplierId });
                if (partSupplier != null && !string.IsNullOrEmpty(partSupplier.NotesForBuyer))
                {
                    notes += $"{partSupplier.NotesForBuyer} {Environment.NewLine}";
                }
            }

            return notes;
        }

        public static DateTime NextOccurrenceOfDay(DateTime from, string deliveryDay)
        {
            var days = new List<string>
                           {
                               "SUNDAY",
                               "MONDAY",
                               "TUESDAY",
                               "WEDNESDAY",
                               "THURSDAY",
                               "FRIDAY",
                               "SATURDAY"
                           };

            var dayIndex = days.IndexOf(deliveryDay);

            var start = (int)from.DayOfWeek;
            var target = dayIndex;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }

        public ProcessResult AuthorisePurchaseOrder(PurchaseOrder order, int userNumber, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderAuthorise, privileges))
            {
                throw new UnauthorisedActionException("You do not have permission to authorise Purchase Orders");
            }

            if (order.AuthorisedById.HasValue)
            {
                return new ProcessResult(false, $"Order {order.OrderNumber} was already authorised");
            }
            else if (this.purchaseOrdersPack.OrderCanBeAuthorisedBy(
                         order.OrderNumber,
                         null,
                         userNumber,
                         null,
                         null,
                         null))
            {
                order.AuthorisedById = userNumber;
                this.AuthoriseMiniOrder(order);

                return new ProcessResult(true, $"Order {order.OrderNumber} successfully authorised");

            }
            else
            {
                return new ProcessResult(false, $"Order {order.OrderNumber} YOU CANNOT AUTHORISE THIS ORDER");
            }
        }

        public ProcessResult AuthoriseMultiplePurchaseOrders(IList<int> orderNumbers, int userNumber)
        {
            if (orderNumbers == null || orderNumbers.Count == 0)
            {
                return new ProcessResult(true, "No orders requested for authorisation");
            }

            var text = string.Empty;
            var success = 0;
            foreach (var orderNumber in orderNumbers)
            {
                var order = this.purchaseOrderRepository.FindById(orderNumber);
                if (order is null)
                {
                    text += $"Order {orderNumber} could not be found\n";
                }
                else if (order.AuthorisedById.HasValue)
                {
                    text += $"Order {orderNumber} was already authorised\n";
                }
                else if (this.purchaseOrdersPack.OrderCanBeAuthorisedBy(
                             orderNumber,
                             null,
                             userNumber,
                             null,
                             null,
                             null))
                {
                    order.AuthorisedById = userNumber;
                    this.AuthoriseMiniOrder(order);
                    text += $"Order {orderNumber} authorised successfully\n";
                    success++;
                }
                else
                {
                    text += $"Order {orderNumber} YOU CANNOT AUTHORISE THIS ORDER\n";
                }
            }

            text += $"\n{success} out of {orderNumbers.Count} authorised successfully";

            return new ProcessResult(true, text);
        }

        public ProcessResult EmailMultiplePurchaseOrders(IList<int> orderNumbers, int userNumber, bool copyToSelf)
        {
            if (orderNumbers == null || orderNumbers.Count == 0)
            {
                return new ProcessResult(true, "No order numbers supplied");
            }

            var text = string.Empty;
            var success = 0;
            foreach (var orderNumber in orderNumbers)
            {
                var order = this.purchaseOrderRepository.FindById(orderNumber);
                var supplierContactEmail = order?.Supplier?.SupplierContacts
                    ?.FirstOrDefault(a => a.IsMainOrderContact == "Y")?.EmailAddress;

                var ccList = order?.Supplier?.SupplierContacts
                    ?.FirstOrDefault(a => a.IsMainOrderContact == "Y")?.OrderCcAddresses;

                if (order is null)
                {
                    text += $"Order {orderNumber} could not be found\n";
                }
                else if (order.Cancelled == "Y")
                {
                    text += $"Order {orderNumber} is cancelled\n";
                }
                else if (order.DocumentType?.Name == "RO")
                {
                    text += $"Order {orderNumber} is a returns order - email from main order page!\n";
                }
                else if (!order.AuthorisedById.HasValue)
                {
                    text += $"Order {orderNumber} is not authorised\n";
                }
                else if (string.IsNullOrEmpty(supplierContactEmail))
                {
                    text += $"Order {orderNumber} could not find order contact email\n";
                }
                else
                {
                    var html = this.purchaseOrderTemplateService.GetHtml(order).Result;
                    try
                    {
                        this.SendOrderPdfEmail(html, supplierContactEmail, copyToSelf, userNumber, order, null, ccList);
                        text += $"Order {orderNumber} emailed successfully to {supplierContactEmail}\n";
                        success++;
                    }
                    catch (MimeKit.ParseException exception)
                    {
                        this.log.Warning($"Order {orderNumber} to {supplierContactEmail} failed with parse exception. {exception.Message}.");
                        text += $"Order {orderNumber} to {supplierContactEmail} failed with parse exception. {exception.Message}. \n";
                    }
                    catch (Exception exception)
                    {
                        this.log.Warning($"Order {orderNumber} to {supplierContactEmail} failed with unknown exception. {exception.Message}.");
                        text += $"Order {orderNumber} to {supplierContactEmail} failed with unknown exception. {exception.Message}. \n";
                    }
                }
            }

            text += $"\n{success} out of {orderNumbers.Count} emailed successfully";
            this.log.Info(text);
            return new ProcessResult(true, text);
        }

        public string GetPurchaseOrderAsHtml(int orderNumber)
        {
            var order = this.GetOrder(orderNumber);
            return this.purchaseOrderTemplateService.GetHtml(order).Result;
        }

        public void CreateMiniOrder(PurchaseOrder order)
        {
            var miniOrder = new MiniOrder();
            var detail = order.Details.First();

            var nomAcc = this.nominalAccountRepository.FindById(detail.OrderPosting.NominalAccountId.Value);

            miniOrder.OrderNumber = order.OrderNumber;
            miniOrder.DocumentType = order.DocumentTypeName;
            miniOrder.DateOfOrder = order.OrderDate;
            miniOrder.RequestedDeliveryDate = detail.PurchaseDeliveries?.First().DateRequested;
            miniOrder.AdvisedDeliveryDate = detail.PurchaseDeliveries?.First().DateAdvised;
            miniOrder.Remarks = order.Remarks;
            miniOrder.SupplierId = order.SupplierId;
            miniOrder.PartNumber = detail.PartNumber;
            miniOrder.DrawingReference = detail.DrawingReference;
            miniOrder.Currency = order.CurrencyCode;
            miniOrder.SuppliersDesignation = detail.SuppliersDesignation;
            miniOrder.Department = nomAcc.DepartmentCode;
            miniOrder.Nominal = nomAcc.NominalCode;
            miniOrder.AuthorisedBy = order.AuthorisedById;
            miniOrder.EnteredBy = order.EnteredById;
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
            miniOrder.OrderMethod = order.OrderMethodName;
            miniOrder.CancelledBy = null;
            miniOrder.SentByMethod = order.SentByMethod;
            miniOrder.AcknowledgeComment = detail.PurchaseDeliveries?.First().SupplierConfirmationComment;
            miniOrder.DeliveryAddressId = order.DeliveryAddressId;
            miniOrder.NumberOfSplitDeliveries = detail.PurchaseDeliveries != null ? detail.PurchaseDeliveries.Count : 0;
            miniOrder.QuotationRef = order.QuotationRef;
            miniOrder.IssuePartsToSupplier = order.IssuePartsToSupplier;
            miniOrder.Vehicle = detail.OrderPosting.Vehicle;
            miniOrder.Building = detail.OrderPosting.Building;
            miniOrder.Product = detail.OrderPosting.Product;
            miniOrder.Person = detail.OrderPosting.Person;
            miniOrder.StockPoolCode = detail.StockPoolCode;
            miniOrder.OurPrice = detail.OurUnitPriceCurrency;
            miniOrder.OrderPrice = detail.OrderUnitPriceCurrency;
            miniOrder.BaseCurrency = order.BaseCurrencyCode;
            miniOrder.BaseOurPrice = detail.BaseOurUnitPrice;
            miniOrder.BaseOrderPrice = detail.BaseOrderUnitPrice;
            miniOrder.BaseNetTotal = detail.BaseNetTotal;
            miniOrder.BaseVatTotal = detail.BaseVatTotal;
            miniOrder.BaseOrderTotal = detail.BaseDetailTotal;
            miniOrder.ExchangeRate = order.ExchangeRate;
            miniOrder.RohsCompliant = detail.RohsCompliant;
            miniOrder.DeliveryConfirmedBy = detail.DeliveryConfirmedById;
            miniOrder.InternalComments = detail.InternalComments;
            miniOrder.PrevOrderNumber = detail.OriginalOrderNumber;
            miniOrder.PrevOrderLine = detail.OriginalOrderLine;
            this.miniOrderRepository.Add(miniOrder);
        }

        public ProcessResult EmailDept(int orderNumber, int userId)
        {
            var sender = this.employeeRepository.FindById(userId);

            if (string.IsNullOrEmpty(sender?.PhoneListEntry?.EmailAddress))
            {
                throw new ItemNotFoundException($"Sender email not found. Check you have one set up in the phone list.");
            }

            var order = this.GetOrder(orderNumber);
            if (order is null)
            {
                throw new ItemNotFoundException($"Could not find order {orderNumber}");
            }

            if (!order.AuthorisedById.HasValue)
            {
                throw new PurchaseOrderException("You cannot email this order until it has been authorised");
            }

            var recipient = order.RequestedBy;

            if (string.IsNullOrEmpty(recipient?.PhoneListEntry?.EmailAddress))
            {
                throw new ItemNotFoundException($"Recipient email not found. Check they have one set up in the phone list.");
            }

            var body =
                "Please click the link when you have received the goods against this order to confirm delivery. \n";
            body += "This will also confirm that payment can be made.  \n";
            body += $"http://app.linn.co.uk/purch/po/podelcon.aspx?po={orderNumber}  \n";
            body += "Any queries regarding this order - please contact a member of the Finance team.";
            this.emailService.SendEmail(
                    recipient.PhoneListEntry.EmailAddress,
                    recipient.FullName,
                    null,
                    null,
                    sender.PhoneListEntry.EmailAddress,
                    sender.FullName,
                    $"Purchase Order {orderNumber} for Supplier {order.Supplier.Name}",
                    body
                );

            return new ProcessResult { Success = true, Message = "Email Request Sent" };
        }

        public PurchaseOrder UnCancelOrder(int orderNumber, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderCancel, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to un-cancel purchase orders");
            }
            var order = this.GetOrder(orderNumber);
            order.Cancelled = "N";
            foreach (var detail in order.Details)
            {
                detail.Cancelled = "N";
                foreach (var c in detail.CancelledDetails)
                {
                    if (!c.DateUncancelled.HasValue)
                    {
                        c.ValueCancelled = 0;
                        c.DateUncancelled = DateTime.Today;
                    }
                }
            }
            var miniOrder = this.miniOrderRepository.FindById(orderNumber);
            miniOrder.CancelledBy = null;
            miniOrder.ReasonCancelled = null;
            return order;
        }

        public PurchaseOrder FilCancelLine(
            int orderNumber,
            int line,
            int filCancelledBy,
            string reasonFilCancelled,
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderFilCancel, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to fil cancel purchase orders");
            }

            var order = this.GetOrder(orderNumber);
            var currentLedgerPeriod = this.purchaseLedgerPack.GetLedgerPeriod();
            var immediateLiability =
                this.liabilityRepository.FindBy(a => a.OrderNumber == orderNumber && a.OrderLine == line);
            var baseImmediateLiability =
                this.baseLiabilityRepository.FindBy(a => a.OrderNumber == orderNumber && a.OrderLine == line);

            var detail = order.Details.First(a => a.Line == line);
            detail.FilCancelled = "Y";
            detail.DateFilCancelled = DateTime.Today;
            detail.PeriodFilCancelled = currentLedgerPeriod;

            var id = this.databaseService.GetIdSequence("PLOC_SEQ");
            var cancelledDetail = new CancelledOrderDetail
                                      {
                                          Id = id,
                                          OrderNumber = orderNumber,
                                          LineNumber = line,
                                          DateFilCancelled = DateTime.Today,
                                          PeriodFilCancelled = currentLedgerPeriod,
                                          FilCancelledById = filCancelledBy,
                                          ReasonFilCancelled = reasonFilCancelled,
                                          ValueFilCancelled = immediateLiability?.Liability ?? 0,
                                          BaseValueFilCancelled = baseImmediateLiability?.Liability ?? 0
                                      };
            this.cancelledOrderDetailRepository.Add(cancelledDetail);

            if (order.Details.Count == 1)
            {
                // only one line so fil cancel the top level order
                order.FilCancelled = "Y";
                order.DateFilCancelled = DateTime.Today;
                order.PeriodFilCancelled = currentLedgerPeriod;
            }

            if (line == 1)
            {
                var miniOrder = this.miniOrderRepository.FindById(orderNumber);
                if (miniOrder is not null)
                {
                    miniOrder.FilCancelledBy = filCancelledBy;
                    miniOrder.ReasonFilCancelled = reasonFilCancelled;
                    miniOrder.DateFilCancelled = DateTime.Today;
                }
            }

            return order;
        }

        public PurchaseOrder UnFilCancelLine(int orderNumber, int line, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderFilCancel, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to fil un-cancel purchase orders");
            }

            var order = this.GetOrder(orderNumber);
            var detail = order.Details.First(a => a.Line == line);
            detail.FilCancelled = "N";
            detail.DateFilCancelled = null;
            detail.PeriodFilCancelled = null;

            var cancelledDetail = detail.CancelledDetails.First(a => a.FilCancelledById.HasValue && !a.DateFilUncancelled.HasValue);
            cancelledDetail.DateFilUncancelled = DateTime.Today;

            order.FilCancelled = "N";
            order.DateFilCancelled = null;
            order.PeriodFilCancelled = null;

            if (line == 1)
            {
                var miniOrder = this.miniOrderRepository.FindById(orderNumber);
                if (miniOrder is not null)
                {
                    miniOrder.FilCancelledBy = null;
                    miniOrder.ReasonFilCancelled = null;
                    miniOrder.DateFilCancelled = null;
                }
            }

            return order;
        }

        public PurchaseOrder SwitchOurQtyAndPrice(
            int orderNumber,
            int orderLine,
            int employeeId,
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseLedgerAdmin, privileges))
            {
                throw new UnauthorisedActionException("No permission to switch qty and price.");
            }

            var order = this.purchaseOrderRepository.FindById(orderNumber);

            var orderLineToUpdate = order.Details.First(a => a.Line == orderLine);

            if (orderLineToUpdate.Part.StockControlled != "N")
            {
                throw new InvalidActionException(
                    $"Cannot switch qty and price on order {orderNumber}/{orderLine} because {orderLineToUpdate.PartNumber} is stock controlled.");
            }

            if (orderLineToUpdate.OurQty != 1m && orderLineToUpdate.OurUnitPriceCurrency != 1m)
            {
                throw new InvalidActionException(
                    $"Cannot switch qty and price on order {orderNumber}/{orderLine} because qty must be 1.");
            }

            if (orderLineToUpdate.PurchaseDeliveries.Any(a => a.QtyNetReceived > 0))
            {
                throw new InvalidActionException(
                    $"Cannot switch qty and price on order {orderNumber}/{orderLine} because deliveries have been received.");
            }

            if (orderLineToUpdate.Cancelled == "Y" || order.Cancelled == "Y")
            {
                throw new InvalidActionException(
                    $"Cannot switch qty and price on order {orderNumber}/{orderLine} because line is cancelled.");
            }

            var originalOurQuantity = orderLineToUpdate.OurQty;
            var originalOurUnitPrice = orderLineToUpdate.OurUnitPriceCurrency;
            var originalBaseOurUnitPrice = orderLineToUpdate.BaseOurUnitPrice;

            if (!originalOurQuantity.HasValue || !originalBaseOurUnitPrice.HasValue || !originalOurUnitPrice.HasValue)
            {
                throw new PurchaseOrderException(
                    $"Cannot switch qty and price on order {orderNumber}/{orderLine}. Price info missing.");
            }

            this.log.Info($"Order {orderNumber}/{orderLine} had qty/price switched on {DateTime.Now:dd-MMM-yyyy} by user {employeeId}.");
            var internalComments = $"Qty/price switched on {DateTime.Now:dd-MMM-yyyy} by user {employeeId}. {orderLineToUpdate.InternalComments}";
            orderLineToUpdate.InternalComments =
                internalComments.Length > 300 ? internalComments.Substring(0, 300) : internalComments;

            orderLineToUpdate.OurQty = originalOurUnitPrice;
            orderLineToUpdate.OurUnitPriceCurrency = originalOurQuantity;
            orderLineToUpdate.BaseOurUnitPrice = decimal.Round(
                originalBaseOurUnitPrice.Value / originalOurUnitPrice.Value,
                5);
            orderLineToUpdate.OrderConversionFactor = originalOurUnitPrice;

            foreach (var purchaseOrderDelivery in orderLineToUpdate.PurchaseDeliveries)
            {
                var originalDeliveryOurQuantity = purchaseOrderDelivery.OurDeliveryQty;
                var originalDeliveryOurUnitPrice = purchaseOrderDelivery.OurUnitPriceCurrency;
                var originalDeliveryBaseOurUnitPrice = purchaseOrderDelivery.BaseOurUnitPrice;

                purchaseOrderDelivery.OurDeliveryQty = originalDeliveryOurUnitPrice;
                purchaseOrderDelivery.OurUnitPriceCurrency = originalDeliveryOurQuantity;
                purchaseOrderDelivery.BaseOurUnitPrice = decimal.Round(
                    originalDeliveryBaseOurUnitPrice.GetValueOrDefault() / originalDeliveryOurUnitPrice.GetValueOrDefault(),
                    5);
            }

            orderLineToUpdate.OrderPosting.Qty = originalOurUnitPrice.Value;

            this.UpdateMiniOrder(order);

            return order;
        }

        private PurchaseOrder GetOrder(int orderNumber)
        {
            var order = this.purchaseOrderRepository.FindById(orderNumber);
            if (order is null)
            {
                throw new ItemNotFoundException($"Could not find order {orderNumber}");
            }

            return order;
        }

        private void AddDeliveryToDetail(PurchaseOrder order, PurchaseOrderDetail detail)
        {
            detail.PurchaseDeliveries = new List<PurchaseOrderDelivery>
                                            {
                                                new PurchaseOrderDelivery
                                                    {
                                                        DeliverySeq = 1,
                                                        OurDeliveryQty = detail.OurQty,
                                                        OrderDeliveryQty = detail.OrderQty,
                                                        OurUnitPriceCurrency = detail.OurUnitPriceCurrency,
                                                        OrderUnitPriceCurrency = detail.OrderUnitPriceCurrency,
                                                        DateRequested = detail.PurchaseDeliveries?.First().DateRequested,
                                                        DateAdvised = null,
                                                        CallOffDate = DateTime.Now,
                                                        Cancelled = "N",
                                                        CallOffRef = null,
                                                        OrderNumber = order.OrderNumber,
                                                        OrderLine = 1,
                                                        FilCancelled = "N",
                                                        NetTotalCurrency = detail.NetTotalCurrency,
                                                        VatTotalCurrency = detail.VatTotalCurrency,
                                                        DeliveryTotalCurrency = detail.NetTotalCurrency,
                                                        SupplierConfirmationComment = string.Empty,
                                                        BaseOurUnitPrice = detail.BaseOurUnitPrice,
                                                        BaseOrderUnitPrice = detail.BaseOrderUnitPrice,
                                                        BaseNetTotal = detail.BaseNetTotal,
                                                        BaseVatTotal = detail.BaseVatTotal,
                                                        BaseDeliveryTotal = detail.BaseNetTotal,
                                                        QuantityOutstanding = detail.OurQty,
                                                        QtyNetReceived = 0,
                                                        QtyPassedForPayment = 0,
                                                        RescheduleReason = "REQUESTED"
                                                    }
                                            };
        }

        private void SetDetailFieldsForCreation(PurchaseOrderDetail detail, int newOrderNumber)
        {
            detail.OrderPosting.Id = this.databaseService.GetIdSequence("PLORP_SEQ");
            detail.OrderPosting.OrderNumber = newOrderNumber;
            detail.OrderNumber = newOrderNumber;
            detail.OrderConversionFactor = 1m;

            // below values are all set as follows in mini order trigger so hardcoding them here for now
            detail.PriceType = "STANDARD";
            detail.Cancelled = "N";
            detail.FilCancelled = "N";
            detail.UpdatePartsupPrice = "N";
            detail.WasPreferredSupplier = "Y";
            detail.IssuePartsToSupplier = "N";

            // required but ignored now that ob ut just uses order fields
            detail.OverbookQtyAllowed = 0;

            // todo check if always LINN or get from table
            detail.StockPoolCode = "LINN";

            detail.RohsCompliant = "Y";
        }

        private void UpdateDeliveries(PurchaseOrderDetail purchaseOrderDetail, decimal exchangeRate, int supplierId)
        {
            foreach (var delivery in purchaseOrderDetail.PurchaseDeliveries)
            {
                if (delivery.QuantityOutstanding > 0)
                {
                    delivery.OrderUnitPriceCurrency = purchaseOrderDetail.OrderUnitPriceCurrency;
                    delivery.OurUnitPriceCurrency = purchaseOrderDetail.OurUnitPriceCurrency;
                    delivery.NetTotalCurrency = Math.Round(delivery.OurDeliveryQty.GetValueOrDefault() * delivery.OurUnitPriceCurrency.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero);
                    delivery.VatTotalCurrency = this.purchaseOrdersPack.GetVatAmountSupplier((decimal)delivery.NetTotalCurrency, supplierId);
                    delivery.BaseOurUnitPrice = purchaseOrderDetail.BaseOurUnitPrice;
                    delivery.BaseOrderUnitPrice = purchaseOrderDetail.BaseOrderUnitPrice;
                    delivery.BaseVatTotal = Math.Round(delivery.VatTotalCurrency.GetValueOrDefault() / exchangeRate, 2, MidpointRounding.AwayFromZero);
                    delivery.BaseNetTotal = Math.Round(delivery.OurDeliveryQty.GetValueOrDefault() * delivery.BaseOurUnitPrice.GetValueOrDefault(), 2);
                    delivery.BaseDeliveryTotal = Math.Round((decimal)((delivery.OurDeliveryQty.GetValueOrDefault() * delivery.BaseOurUnitPrice.GetValueOrDefault()) + delivery.BaseVatTotal), 2);
                    delivery.DeliveryTotalCurrency = Math.Round((decimal)delivery.NetTotalCurrency + delivery.VatTotalCurrency.Value, 2);
                }
            }
        }

        private void UpdateDetailProperties(
            PurchaseOrderDetail current,
            PurchaseOrderDetail updated,
            int supplierId,
            decimal exchangeRate,
            string documentType)
        {
            current.SuppliersDesignation = updated.SuppliersDesignation;
            current.InternalComments = updated.InternalComments;

            var updatedNominal = updated.OrderPosting.NominalAccount;

            var nominalAccounts = this.nominalAccountRepository.FilterBy(x => x.NominalCode == updatedNominal.NominalCode);

            if (!nominalAccounts.Any())
            {
                nominalAccounts = this.nominalAccountRepository.FilterBy(
                    x => x.NominalCode.EndsWith(updatedNominal.NominalCode));
            }

            var updatedNominalAccount = nominalAccounts.FirstOrDefault(
                                     x => x.DepartmentCode == updatedNominal.DepartmentCode)
                                 ?? nominalAccounts.FirstOrDefault(
                                     x => x.DepartmentCode.EndsWith(updatedNominal.DepartmentCode));

            if (updatedNominalAccount == null)
            {
                throw new ItemNotFoundException("Invalid nominal code/dept");
            }


            if (current.OrderPosting.NominalAccount.AccountId != updatedNominalAccount.AccountId
                && current.PurchaseDeliveries.Any(d => d.QtyNetReceived > 0))
            {
                throw new PurchaseOrderException(
                    "Cannot update nominal account after some qty of the order has been booked in");
            }

            current.OrderPosting.NominalAccount = updatedNominalAccount;

            if ((documentType != "CO") && (documentType != "RO"))
            {
                this.PerformDetailCalculations(current, updated, exchangeRate, supplierId);
            }

            this.UpdateOrderPostingsForDetail(current, updated);

            this.UpdateDeliveries(current, exchangeRate, supplierId);
        }

        private void PerformDetailCalculations(PurchaseOrderDetail current, PurchaseOrderDetail updated, decimal exchangeRate, int supplierId, bool creating = false)
        {
            var netTotal = updated.OurUnitPriceCurrency.GetValueOrDefault() * updated.OurQty.GetValueOrDefault();
            current.NetTotalCurrency = Math.Round(netTotal, 2, MidpointRounding.AwayFromZero);

            current.VatTotalCurrency =
                this.purchaseOrdersPack.GetVatAmountSupplier(current.NetTotalCurrency, supplierId);

            current.DetailTotalCurrency = Math.Round(netTotal + current.VatTotalCurrency.Value, 2);

            if (creating || (updated.OrderUnitPriceCurrency == current.OrderUnitPriceCurrency
                && updated.OurUnitPriceCurrency != current.OurUnitPriceCurrency))
            {
                // if order price hasn't been overridden but our price has changed, use conv factor
                current.OrderUnitPriceCurrency = updated.OurUnitPriceCurrency * current.OrderConversionFactor;
            }
            else if (updated.OrderUnitPriceCurrency != current.OrderUnitPriceCurrency)
            {
                // if order price has been manually overridden
                current.OrderUnitPriceCurrency = updated.OrderUnitPriceCurrency;
            }

            if (creating || (updated.OrderQty == current.OrderQty && updated.OurQty != current.OurQty))
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
                5,
                MidpointRounding.AwayFromZero);
            current.BaseOurUnitPrice = Math.Round(
                current.OurUnitPriceCurrency.GetValueOrDefault() / exchangeRate,
                5,
                MidpointRounding.AwayFromZero);

            current.BaseVatTotal = Math.Round(
                current.VatTotalCurrency.GetValueOrDefault() / exchangeRate,
                2,
                MidpointRounding.AwayFromZero);

            current.BaseDetailTotal = Math.Round(
                current.DetailTotalCurrency.GetValueOrDefault() / exchangeRate,
                2,
                MidpointRounding.AwayFromZero);
        }

        private void UpdateDetails(
            ICollection<PurchaseOrderDetail> currentDetails,
            ICollection<PurchaseOrderDetail> updatedDetails,
            int supplierId,
            decimal exchangeRate,
            string documentType)
        {
            foreach (var detail in updatedDetails)
            {
                var currentDetail = currentDetails.FirstOrDefault(x => x.Line == detail.Line);
                if (currentDetail != null)
                {
                    this.UpdateDetailProperties(currentDetail, detail, supplierId, exchangeRate, documentType);
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
            miniOrder.SentByMethod = updatedOrder.SentByMethod;

            var nomAcc = this.nominalAccountRepository.FindById(updatedDetail.OrderPosting.NominalAccountId.Value);
            miniOrder.Nominal = nomAcc.NominalCode;
            miniOrder.Department = nomAcc.DepartmentCode;

            miniOrder.RequestedDeliveryDate = updatedDetail.PurchaseDeliveries.First().DateRequested;
            miniOrder.InternalComments = updatedDetail.InternalComments;
            miniOrder.SuppliersDesignation = updatedDetail.SuppliersDesignation;
            miniOrder.DrawingReference = updatedDetail.DrawingReference;

            miniOrder.NetTotal = updatedDetail.NetTotalCurrency;
            miniOrder.VatTotal = updatedDetail.VatTotalCurrency.GetValueOrDefault();
            miniOrder.OrderTotal = Math.Round(miniOrder.NetTotal + miniOrder.VatTotal, 2);

            miniOrder.OurQty = updatedDetail.OurQty;
            miniOrder.OrderQty = updatedDetail.OrderQty;
            miniOrder.OurPrice = updatedDetail.OurUnitPriceCurrency;
            miniOrder.OrderPrice = updatedDetail.OrderUnitPriceCurrency;

            miniOrder.OurUnitOfMeasure = updatedDetail.OurUnitOfMeasure;
            miniOrder.OrderConvFactor = updatedDetail.OrderConversionFactor;

            miniOrder.BaseOrderPrice = updatedDetail.BaseOrderUnitPrice;
            miniOrder.BaseOurPrice = updatedDetail.BaseOurUnitPrice;

            miniOrder.BaseNetTotal = updatedDetail.BaseNetTotal;
            miniOrder.BaseVatTotal = updatedDetail.BaseVatTotal;
            miniOrder.BaseOrderTotal = updatedDetail.BaseDetailTotal;
        }

        private void AuthoriseMiniOrder(PurchaseOrder updatedOrder)
        {
            var miniOrder = this.miniOrderRepository.FindById(updatedOrder.OrderNumber);
            miniOrder.AuthorisedBy = updatedOrder.AuthorisedById;
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
            current.SentByMethod = updated.SentByMethod;
        }

        private void SendOrderPdfEmail(
            string html,
            string emailAddress,
            bool bcc,
            int currentUserId,
            PurchaseOrder order,
            string debitNoteHtml,
            string ccList = null)
        {
            var orderPdf = this.pdfService.ConvertHtmlToPdf(html, false);

            var emailBody = $"Please accept the attached order no. {order.OrderNumber}.\n"
                            + "Linn's standard Terms & Conditions apply at all times\n"
                            + "and can be found at www.linn.co.uk/purchasing_conditions.\n"
                            + "This email is sent from an unmonitored account. Please send all correspondence direct to the buyer.";

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

            List<Dictionary<string, string>> cc = new List<Dictionary<string, string>>();

            if (!string.IsNullOrEmpty(ccList))
            {
                var ccArray = ccList.Split(',');
                foreach (var s in ccArray)
                {
                    if (!string.IsNullOrEmpty(s.Trim()))
                    {
                        cc.Add(new Dictionary<string, string>
                                   {
                                       { "name", s }, { "address", s }
                                   });
                    }
                }
            }

            var attachments = new List<Attachment>
                                  {
                                      new PdfAttachment(orderPdf.Result, $"LinnPurchaseOrder{order.OrderNumber}")
                                  };

            if (debitNoteHtml != null)
            {
                var debitNotePdf = this.pdfService.ConvertHtmlToPdf(debitNoteHtml, false);
                attachments.Add(new PdfAttachment(debitNotePdf.Result, $"DebitNote"));
                if (order.Supplier.AccountController?.PhoneListEntry != null)
                {
                    bccList.Add(
                        new Dictionary<string, string>
                            {
                                { "name", order.Supplier.AccountController.FullName },
                                { "address", order.Supplier.AccountController.PhoneListEntry.EmailAddress }
                            });
                }
            }

            this.emailService.SendEmail(
                    emailAddress,
                    emailAddress,
                    cc,
                    bccList,
                    ConfigurationManager.Configuration["PURCHASING_FROM_ADDRESS"],
                    "Linn Purchasing",
                    $"Linn Purchase Order {order.OrderNumber}",
                    emailBody,
                   attachments);

            order.SentByMethod = "EMAIL";

            //// todo When get rid of mini orders remove below
            var miniOrder = this.miniOrderRepository.FindById(order.OrderNumber);
            if (miniOrder != null)
            {
                miniOrder.SentByMethod = "EMAIL";
            }

            this.log.Write(
                LoggingLevel.Info,
                new List<LoggingProperty>(),
                $"Email sent for purchase order {order.OrderNumber} to {emailAddress}");
        }

        private void CheckOkToRaiseOrders()
        {
            if (!this.purchaseLedgerMaster.GetRecord().OkToRaiseOrder.Equals("Y"))
            {
                throw new UnauthorisedActionException("Orders are currently restricted.");
            }
        }
    }
}
