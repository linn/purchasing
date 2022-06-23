namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
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
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IAuthorisationService authService;

        private readonly IDatabaseService databaseService;

        private readonly IPurchaseLedgerPack purchaseLedgerPack;

        private readonly IPdfService pdfService;

        private readonly IEmailService emailService;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<MiniOrder, int> miniOrderRepository;

        public PurchaseOrderService(
            IAuthorisationService authService,
            IPurchaseLedgerPack purchaseLedgerPack,
            IDatabaseService databaseService,
            IPdfService pdfService,
            IEmailService emailService,
            IRepository<Employee, int> employeeRepository,
            IRepository<MiniOrder, int> miniOrderRepository)
        {
            this.authService = authService;
            this.purchaseLedgerPack = purchaseLedgerPack;
            this.databaseService = databaseService;
            this.pdfService = pdfService;
            this.emailService = emailService;
            this.employeeRepository = employeeRepository;
            this.miniOrderRepository = miniOrderRepository;
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

        public void CreateOrder(PurchaseOrder order, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderCreate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to create purchase orders");
            }

            // add id to pl_order_postings using next val plorp_seq
            // select nomacc_id
            // from nominal_accounts where
            //    nominal = p_nom and department = p_dept;
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

        public PurchaseOrder UpdateOrder(PurchaseOrder current, PurchaseOrder updated, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update purchase orders");
            }

            this.UpdateOrderProperties(current, updated);
            this.UpdateDetails(current.Details, updated.Details);
            // Update pl_order_postings? Or just on create? todo investigate
            return current;
        }

        public ProcessResult SendPdfEmail(string html, string emailAddress, int orderNumber, bool bcc, int currentUserId, PurchaseOrder order)
        {
            var pdf = this.pdfService.ConvertHtmlToPdf(html, landscape: false);
            var emailBody = $"Please accept the attached order no. {orderNumber}.\n"
                            + $"You will need Acrobat Reader to open the file which is available from www.adobe.com/acrobat\n"
                            + "Linn's standard Terms & Conditions apply at all times\n"
                            + "and can be found at www.linn.co.uk/purchasing_conditions";

            var bccList = new List<Dictionary<string, string>>
                              {
                                  new Dictionary<string, string>
                                      {
                                          { "name", "purchasing outgoing" }, { "address", ConfigurationManager.Configuration["PURCHASING_FROM_ADDRESS"] }
                                      }
                              };
            if (bcc)
            {
                var employee = this.employeeRepository.FindById(currentUserId);
                bccList.Add(new Dictionary<string, string> { { "name", employee.FullName }, { "address", employee.PhoneListEntry?.EmailAddress } });
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
                    pdf.Result,
                    $"LinnPurchaseOrder{orderNumber}");

            var miniOrder = this.miniOrderRepository.FindById(orderNumber);
            miniOrder.SentByMethod = "EMAIL";
            //// mini order trigger will update pl order. When remove mini orders:
            //// todo stop setting sentbymethod mini order and switch to below
            //// order.SentByMethod = "EMAIL";

            return new ProcessResult(true, "Email Sent");
        }

        private void UpdateDetails(ICollection<PurchaseOrderDetail> currentDetails, ICollection<PurchaseOrderDetail> updatedDetails)
        {
            foreach (var detail in updatedDetails)
            {
                var currentDetail = currentDetails.FirstOrDefault(x => x.Line == detail.Line);
                if (currentDetail != null)
                {
                    this.UpdateDetailProperties(currentDetail, detail);
                }
                else
                {
                    currentDetails.Add(detail);
                }
            }
        }

        private void UpdateOrderProperties(PurchaseOrder current, PurchaseOrder updated)
        {
            current.Cancelled = updated.Cancelled;
            current.DocumentTypeName = updated.DocumentTypeName;
            current.OrderDate = updated.OrderDate;
            current.SupplierId = updated.SupplierId;
            current.CurrencyCode = updated.CurrencyCode;
            current.OrderContactName = updated.OrderContactName;
            current.OrderMethodName = updated.OrderMethodName;
            current.ExchangeRate = updated.ExchangeRate;
            current.IssuePartsToSupplier = updated.IssuePartsToSupplier;
            current.DeliveryAddressId = updated.DeliveryAddressId;
            current.RequestedById = updated.RequestedById;
            current.EnteredById = updated.EnteredById;
            current.QuotationRef = updated.QuotationRef;
            current.AuthorisedById = updated.AuthorisedById;
            current.SentByMethod = updated.SentByMethod;
            current.FilCancelled = updated.FilCancelled;
            current.Remarks = updated.Remarks;
            current.DateFilCancelled = updated.DateFilCancelled;
            current.PeriodFilCancelled = updated.PeriodFilCancelled;
        }

        private void UpdateDetailProperties(PurchaseOrderDetail current, PurchaseOrderDetail updated)
        {
            current.PartNumber = updated.PartNumber;
            current.Cancelled = updated.Cancelled;
            current.BaseNetTotal = updated.BaseNetTotal;
            current.NetTotalCurrency = updated.NetTotalCurrency;
            current.OurQty = updated.OurQty;
            current.PartNumber = updated.PartNumber;
            current.RohsCompliant = updated.RohsCompliant;
            current.SuppliersDesignation = updated.SuppliersDesignation;
            current.StockPoolCode = updated.StockPoolCode;
            current.OriginalOrderNumber = updated.OriginalOrderNumber;
            current.OriginalOrderLine = updated.OriginalOrderLine;
            current.OurUnitOfMeasure = updated.OurUnitOfMeasure;
            current.OrderUnitOfMeasure = updated.OrderUnitOfMeasure;
            current.Duty = updated.Duty;
            current.OurUnitPriceCurrency = updated.OurUnitPriceCurrency;
            current.OrderUnitPriceCurrency = updated.OrderUnitPriceCurrency;
            current.BaseOurUnitPrice = updated.BaseOurUnitPrice;
            current.BaseOrderUnitPrice = updated.BaseOrderUnitPrice;
            current.VatTotalCurrency = updated.VatTotalCurrency;
            current.BaseVatTotal = updated.BaseVatTotal;
            current.DetailTotalCurrency = updated.DetailTotalCurrency;
            current.BaseDetailTotal = updated.BaseDetailTotal;
            current.DeliveryInstructions = updated.DeliveryInstructions;
            current.DeliveryConfirmedBy = updated.DeliveryConfirmedBy;
            current.DeliveryConfirmedById = updated.DeliveryConfirmedById;
            current.InternalComments = updated.InternalComments;
        }
    }
}
