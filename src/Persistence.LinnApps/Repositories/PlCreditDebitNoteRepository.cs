namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using Microsoft.EntityFrameworkCore;

    public class PlCreditDebitNoteRepository : EntityFrameworkRepository<PlCreditDebitNote, int>
    {
        public PlCreditDebitNoteRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.PlCreditDebitNotes)
        {
        }

        public override PlCreditDebitNote FindById(int key)
        {
            return this.FindAll()
                .Include(x => x.NoteType)
                .Include(x => x.Supplier).ThenInclude(s => s.OrderFullAddress)
                .Include(x => x.Supplier).ThenInclude(s => s.SupplierContacts).ThenInclude(c => c.Person)
                .Include(x => x.PurchaseOrder).ThenInclude(o => o.Details).ThenInclude(d => d.Part)
                .Include(x => x.Currency)
                .FirstOrDefault(x => x.NoteNumber == key);
        }

        public override IQueryable<PlCreditDebitNote> FilterBy(
            Expression<Func<PlCreditDebitNote, bool>> expression)
        {
            return base.FilterBy(expression)
                .Include(x => x.NoteType)
                .Include(n => n.Supplier)
                .Include(x => x.PurchaseOrder).AsNoTracking();
        }
    }
}
