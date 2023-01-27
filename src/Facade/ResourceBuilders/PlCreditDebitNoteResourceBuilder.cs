namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class PlCreditDebitNoteResourceBuilder : IBuilder<PlCreditDebitNote>
    {
        private readonly IAuthorisationService authService;

        public PlCreditDebitNoteResourceBuilder(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public PlCreditDebitNoteResource Build(PlCreditDebitNote note, IEnumerable<string> claims)
        {
            return new PlCreditDebitNoteResource
                   {
                       OrderQty = note.OrderQty,
                       PartNumber = note.PartNumber,
                       DateClosed = note.DateClosed?.ToString("o"),
                       SupplierId = note.Supplier.SupplierId,
                       ClosedBy = note.ClosedBy,
                       NetTotal = note.NetTotal,
                       NoteNumber = note.NoteNumber,
                       OriginalOrderNumber = note.PurchaseOrder?.OrderNumber,
                       ReturnsOrderNumber = note.ReturnsOrderNumber,
                       ReturnsOrderLine = note.ReturnsOrderLine,
                       Notes = note.Notes,
                       SupplierName = note.Supplier?.Name,
                       DateCreated = note.DateCreated.ToString("o"),
                       SupplierFullAddress = note.Supplier.OrderAddress?.FullAddress?.AddressString,
                       OrderUnitOfMeasure = note.OrderUnitOfMeasure,
                       OrderUnitPrice = note.OrderUnitPrice,
                       Total = note.Total,
                       VatTotal = note.VatTotal,
                       SuppliersDesignation = note.SuppliersDesignation,
                       OrderContactName = note.PurchaseOrder?.OrderContactName,
                       SupplierAddress = note.Supplier.OrderAddress?.FullAddress?.AddressString,
                       Currency = note.Currency?.Name,
                       OrderDetails = note.PurchaseOrder?.Details
                           ?.Select(d => new PurchaseOrderDetailResource
                                             {
                                                 Line = d.Line,
                                                 PartNumber = d.Part?.PartNumber,
                                                 PartDescription = d.Part?.Description
                                             }),
                       VatRate = note.VatRate,
                       Cancelled = note.CancelledBy.HasValue,
                       NoteType = note.NoteType.Type,
                       TypePrintDescription = note.NoteType.PrintDescription,
                       Links = this.BuildLinks(note, claims).ToArray()
                   };
        }

        public string GetLocation(PlCreditDebitNote p)
        {
            return $"/purchasing/pl-credit-debit-notes/{p.NoteNumber}";
        }

        object IBuilder<PlCreditDebitNote>.Build(PlCreditDebitNote entity, IEnumerable<string> claims) 
            => this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(PlCreditDebitNote model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }

            if (this.authService.HasPermissionFor(AuthorisedAction.PlCreditDebitNoteCreate, claims))
            {
                yield return new LinkResource { Rel = "create", Href = $"/purchasing/pl-credit-debit-notes/create" };
            }
        }
    }
}
