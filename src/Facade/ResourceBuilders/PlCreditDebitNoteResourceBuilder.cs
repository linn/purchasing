namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
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
                       NoteType = note.NoteType
                   };
    }

    public string GetLocation(PlCreditDebitNote p)
    {
        throw new NotImplementedException();
    }

    object IBuilder<PlCreditDebitNote>.Build(PlCreditDebitNote entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
