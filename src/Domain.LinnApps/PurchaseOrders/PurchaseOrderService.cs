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
            this.UpdateMiniOrder(updated);
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
            current.Remarks = updated.Remarks;
        }

        private void UpdateDetailProperties(PurchaseOrderDetail current, PurchaseOrderDetail updated)
        {
            current.BaseNetTotal = updated.BaseNetTotal;
            current.NetTotalCurrency = updated.NetTotalCurrency;
            current.OurQty = updated.OurQty;
            current.OrderQty = updated.OrderQty;

            current.SuppliersDesignation = updated.SuppliersDesignation;

            // price updates to be done next
            // current.OurUnitPriceCurrency = updated.OurUnitPriceCurrency;
            // current.OrderUnitPriceCurrency = updated.OrderUnitPriceCurrency;
            // current.BaseOrderUnitPrice = updated.BaseOrderUnitPrice;
            // current.BaseOurUnitPrice = updated.BaseOurUnitPrice;
            // current.VatTotalCurrency = updated.VatTotalCurrency;
            // current.BaseVatTotal = updated.BaseVatTotal; //// vat totals might not change
            // current.DetailTotalCurrency = updated.DetailTotalCurrency;
            // current.BaseDetailTotal = updated.BaseDetailTotal;
            current.InternalComments = updated.InternalComments;

            this.UpdateOrderPostingsForDetail(current, updated);

            this.UpdateDeliveries(current.PurchaseDeliveries, updated.PurchaseDeliveries);
        }

        private void UpdateDeliveries(ICollection<PurchaseOrderDelivery> deliveries, ICollection<PurchaseOrderDelivery> updatedDeliveries)
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

            private void UpdateMiniOrder(PurchaseOrder updatedOrder)
        {
            var miniOrder = this.miniOrderRepository.FindById(updatedOrder.OrderNumber);
            var updatedDetail = updatedOrder.Details.First();

            miniOrder.OurQty = updatedDetail.OurQty;
            miniOrder.OrderQty = updatedDetail.OrderQty;

            miniOrder.OurPrice = updatedDetail.OurUnitPriceCurrency;
            miniOrder.OrderPrice = updatedDetail.OrderUnitPriceCurrency;
            miniOrder.NetTotal = updatedDetail.NetTotalCurrency;
            miniOrder.BaseOurPrice = updatedDetail.BaseOurUnitPrice;
            miniOrder.BaseOrderPrice = updatedDetail.BaseOrderUnitPrice;
            miniOrder.BaseNetTotal = updatedDetail.BaseNetTotal;

            miniOrder.Remarks = updatedOrder.Remarks;
            miniOrder.Department = updatedDetail.OrderPosting.NominalAccount.Department.DepartmentCode;
            miniOrder.Nominal = updatedDetail.OrderPosting.NominalAccount.Nominal.NominalCode;
            miniOrder.RequestedDeliveryDate = updatedDetail.PurchaseDeliveries.First().DateRequested;
            miniOrder.InternalComments = updatedDetail.InternalComments;
            miniOrder.SuppliersDesignation = updatedDetail.SuppliersDesignation;
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

        private void UpdateOrderPostingsForDetail(PurchaseOrderDetail current, PurchaseOrderDetail updated)
        {
            if (current.OrderPosting.NominalAccountId != updated.OrderPosting.NominalAccountId)
            {
                current.OrderPosting.NominalAccountId = updated.OrderPosting.NominalAccountId;
            }
        }
    }
}
