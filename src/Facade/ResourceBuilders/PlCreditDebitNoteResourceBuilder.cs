namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class PlCreditDebitNoteResourceBuilder : IBuilder<PlCreditDebitNote>
    {
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
                       OriginalOrderNumber = note.OriginalOrderNumber,
                       ReturnsOrderNumber = note.ReturnsOrderNumber,
                       Notes = note.Notes,
                       SupplierName = note.Supplier?.Name,
                       DateCreated = note.DateCreated.ToString("o"),
                       NoteType = note.NoteType,
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
            yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
        }
    }
}
