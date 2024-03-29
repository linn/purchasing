﻿namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Email;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PlCreditDebitNoteService : IPlCreditDebitNoteService
    {
        private readonly IAuthorisationService authService;

        private readonly IEmailService emailService;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<PlCreditDebitNote, int> repository;

        private readonly IRepository<Supplier, int> supplierRepository;

        private readonly IDatabaseService databaseService;

        private readonly ISalesTaxPack salesTaxPack;

        private readonly IRepository<CreditDebitNoteType, string> noteTypesRepository;

        private readonly IRepository<Currency, string> currencyRepository;

        private readonly IRepository<PurchaseOrder, int> purchaseOrderRepository;

        public PlCreditDebitNoteService(
            IAuthorisationService authService,
            IEmailService emailService,
            IRepository<Employee, int> employeeRepository,
            IRepository<PlCreditDebitNote, int> repository,
            ISalesTaxPack salesTaxPack,
            IRepository<Supplier, int> supplierRepository,
            IDatabaseService databaseService,
            IRepository<CreditDebitNoteType, string> noteTypesRepository,
            IRepository<Currency, string> currencyRepository,
            IRepository<PurchaseOrder, int> purchaseOrderRepository)
        {
            this.authService = authService;
            this.emailService = emailService;
            this.employeeRepository = employeeRepository;
            this.repository = repository;
            this.salesTaxPack = salesTaxPack;
            this.databaseService = databaseService;
            this.supplierRepository = supplierRepository;
            this.noteTypesRepository = noteTypesRepository;
            this.currencyRepository = currencyRepository;
            this.purchaseOrderRepository = purchaseOrderRepository;
        }

        public void CloseDebitNote(
            PlCreditDebitNote toClose, 
            string reason,
            int closedBy,
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PlCreditDebitNoteClose, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to close debit notes");
            }

            toClose.DateClosed = DateTime.Today;
            toClose.ReasonClosed = reason;
            toClose.ClosedBy = closedBy;
        }

        public void CancelDebitNote(
            PlCreditDebitNote toCancel,
            string reason, 
            int cancelledBy, 
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PlCreditDebitNoteCancel, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to cancel debit notes");
            }

            toCancel.DateCancelled = DateTime.Today;
            toCancel.ReasonCancelled = reason;
            toCancel.CancelledBy = cancelledBy;
        }

        public void UpdatePlCreditDebitNote(
            PlCreditDebitNote current, PlCreditDebitNote updated, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PlCreditDebitNoteUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update credit/debit notes");
            }

            current.Notes = updated.Notes;
        }

        public ProcessResult SendEmails(
            Employee sender, 
            PlCreditDebitNote note, 
            Stream pdfAttachment)
        {
            var contact = note.Supplier?.SupplierContacts?.FirstOrDefault(c => c.IsMainOrderContact == "Y");

            if (contact == null)
            {
                return new ProcessResult 
                           {
                               Success = false,
                               Message = "Supplier has no main order contact"
                           };
            }

            if (string.IsNullOrEmpty(sender?.PhoneListEntry?.EmailAddress))
            {
                return new ProcessResult
                           {
                               Success = false,
                               Message = "Cannot find sender email address"
                           };
            }

            var bccFinancePersonId = note.Supplier.Country.Equals("GB") ? 33039 : 6001;

            var bccFinancePerson = this.employeeRepository.FindById(bccFinancePersonId);

            var bccEntry = new Dictionary<string, string>
                               {
                                   { "name", bccFinancePerson.FullName},
                                   { "address", bccFinancePerson.PhoneListEntry.EmailAddress }
                               };

            var bccList = new List<Dictionary<string, string>>
                              {
                                  bccEntry
                              };

            try
            {
                this.emailService.SendEmail(
                    contact.EmailAddress.Trim(),
                    $"{contact.Person.FirstName} {contact.Person.LastName}",
                    null,
                    bccList,
                    sender.PhoneListEntry.EmailAddress.Trim(),
                    sender.FullName,
                    $"Linn Products {note.NoteType.PrintDescription} {note.NoteNumber}",
                    $"Attached is a copy of Linn Products {note.NoteType.PrintDescription} {note.NoteNumber}",
                    new List<Attachment>
                        {
                            new PdfAttachment(pdfAttachment, $"{note.NoteType.PrintDescription} {note.NoteNumber}")
                        });

                return new ProcessResult(true, "Email Sent");
            }
            catch (Exception e)
            {
                return new ProcessResult
                           {
                               Success = false,
                               Message = $"Error sending email. Error Message: {e.Message}"
                           };
            }
        }

        public void CreateDebitOrCreditNoteFromPurchaseOrder(PurchaseOrder order)
        {
            if (order.DocumentTypeName == "CO" || order.DocumentTypeName == "RO")
            {
                PurchaseOrder originalOrder;
                var originalOrderNumber = order.Details.First().OriginalOrderNumber;
                if (originalOrderNumber != null)
                {
                    originalOrder =
                        this.purchaseOrderRepository.FindById(originalOrderNumber.Value);
                }
                else
                {
                    throw new PurchaseOrderException(
                        $"Cannot create a debit note for order {order.OrderNumber} as no original order specified");
                }

                var unitPrice = originalOrder.Details.First().OrderUnitPriceCurrency.GetValueOrDefault();
                var netTotal = Math.Round(
                    unitPrice * order.Details.First().OrderQty.GetValueOrDefault(),
                    2,
                    MidpointRounding.AwayFromZero);
                var vatRate = this.salesTaxPack.GetVatRateSupplier(order.SupplierId);
                var vatTotal = Math.Round(
                    netTotal * vatRate / 100,
                    2,
                    MidpointRounding.AwayFromZero);
                var total = netTotal + vatTotal;
                var note = new PlCreditDebitNote
                               {
                                   CreatedBy = order.EnteredById,
                                   NoteNumber = this.databaseService.GetNextVal("PLCDN_SEQ"),
                                   PartNumber = order.Details.First().PartNumber,
                                   OrderQty = order.Details.First().OrderQty.GetValueOrDefault(),
                                   ReturnsOrderNumber = order.OrderNumber,
                                   ReturnsOrderLine = 1,
                                   NetTotal = netTotal,
                                   Total = total,
                                   OrderUnitPrice = originalOrder.Details.First().OrderUnitPriceCurrency.GetValueOrDefault(),
                                   OrderUnitOfMeasure = originalOrder.Details.First().OrderUnitOfMeasure,
                                   VatTotal = vatTotal,
                                   Notes = null,
                                   DateClosed = null,
                                   DateCreated = DateTime.Now,
                                   ClosedBy = null,
                                   ReasonClosed = null,
                                   Supplier = this.supplierRepository.FindById(order.SupplierId),
                                   SuppliersDesignation = order.Details.First().SuppliersDesignation,
                                   Currency = order.Currency,
                                   VatRate = vatRate,
                                   CancelledBy = null,
                                   DateCancelled = null,
                                   ReasonCancelled = null,
                                   NoteType = this.noteTypesRepository.FindById("D"),
                                   CreditOrReplace = order.DocumentTypeName == "CO" ? "CREDIT" : "REPLACE",
                                   OriginalOrderNumber = originalOrderNumber,
                                   OriginalOrderLine = order.Details.First().OriginalOrderLine
                               };

                var details = new List<PlCreditDebitNoteDetail>();

                foreach (var purchaseOrderDetail in order.Details)
                {
                    originalOrderNumber = purchaseOrderDetail.OriginalOrderNumber;
                    if (originalOrderNumber != null)
                    {
                        originalOrder = this.purchaseOrderRepository.FindById(originalOrderNumber.Value);
                    }
                    else
                    {
                        throw new PurchaseOrderException(
                            $"Cannot create a debit note for order {order.OrderNumber} as no original order specified");
                    }

                    unitPrice = originalOrder.Details.First(a => a.Line == purchaseOrderDetail.OriginalOrderLine).OrderUnitPriceCurrency.GetValueOrDefault();
                    netTotal = Math.Round(
                        unitPrice * purchaseOrderDetail.OrderQty.GetValueOrDefault(),
                        2,
                        MidpointRounding.AwayFromZero);
                    vatRate = this.salesTaxPack.GetVatRateSupplier(order.SupplierId);
                    vatTotal = Math.Round(
                        netTotal * vatRate / 100,
                        2,
                        MidpointRounding.AwayFromZero);
                    total = netTotal + vatTotal;
                    details.Add(new PlCreditDebitNoteDetail
                                    {
                                        NoteNumber = note.NoteNumber,
                                        LineNumber = purchaseOrderDetail.Line,
                                        PartNumber = purchaseOrderDetail.PartNumber,
                                        OrderQty = purchaseOrderDetail.OrderQty.GetValueOrDefault(),
                                        OriginalOrderLine = purchaseOrderDetail.OriginalOrderLine,
                                        ReturnsOrderLine = purchaseOrderDetail.Line,
                                        NetTotal = netTotal,
                                        Total = total,
                                        OrderUnitPrice = unitPrice,
                                        OrderUnitOfMeasure = purchaseOrderDetail.OrderUnitOfMeasure,
                                        VatTotal = vatTotal,
                                        Notes = null,
                                        SuppliersDesignation = purchaseOrderDetail.SuppliersDesignation,
                                        Header = note
                                    });
                }

                note.Details = details;

                this.repository.Add(note);
            }
        }

        public PlCreditDebitNote CreateCreditNote(PlCreditDebitNote candidate, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PlCreditDebitNoteCreate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to create purchase ledger notes");
            }

            candidate.NoteNumber = this.databaseService.GetNextVal("PLCDN_SEQ");

            candidate.NoteType = candidate.ReturnsOrderNumber.HasValue 
                                     ? this.noteTypesRepository.FindById("C") : this.noteTypesRepository.FindById("F");
            candidate.DateCreated = DateTime.Today;
            candidate.PurchaseOrder = candidate.OriginalOrderNumber.HasValue 
                                          ? this.purchaseOrderRepository.FindById((int)candidate.OriginalOrderNumber) : null;
            
            candidate.Supplier = this.supplierRepository.FindById(candidate.Supplier.SupplierId);
            
            if (candidate.Supplier == null)
            {
                throw new ItemNotFoundException("SupplierId not recognised");
            }

            candidate.Currency = this.currencyRepository.FindById(candidate.Currency.Code.ToUpper());

            if (candidate.Currency == null)
            {
                throw new ItemNotFoundException("Currency code not recognised");
            }

            candidate.VatRate = this.salesTaxPack.GetVatRateSupplier(candidate.Supplier.SupplierId);
            candidate.SuppliersDesignation = candidate.PurchaseOrder?.Details
                .First(d => d.Line == candidate.OriginalOrderLine).SuppliersDesignation ?? candidate.PartNumber;

            candidate.Details = new List<PlCreditDebitNoteDetail>
                                    {
                                        new PlCreditDebitNoteDetail
                                            {
                                                NoteNumber = candidate.NoteNumber,
                                                LineNumber = 1,
                                                PartNumber = candidate.PartNumber,
                                                OrderQty = candidate.OrderQty,
                                                OriginalOrderLine = candidate.OriginalOrderLine,
                                                ReturnsOrderLine = candidate.ReturnsOrderLine,
                                                NetTotal = candidate.NetTotal,
                                                Total = candidate.Total,
                                                OrderUnitPrice = candidate.OrderUnitPrice,
                                                OrderUnitOfMeasure = candidate.OrderUnitOfMeasure,
                                                VatTotal = candidate.VatTotal,
                                                Notes = candidate.Notes,
                                                SuppliersDesignation = candidate.SuppliersDesignation,
                                                Header = candidate
                                            }
                                    };

            return candidate;
        }
    }
}
