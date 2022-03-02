﻿namespace Linn.Purchasing.Persistence.LinnApps.Repositories
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
            return this.FindAll().Include(x => x.Supplier).FirstOrDefault(x => x.NoteNumber == key);
        }

        public override IQueryable<PlCreditDebitNote> FilterBy(Expression<Func<PlCreditDebitNote, bool>> expression)
        {
            return base.FilterBy(expression).Include(n => n.Supplier);
        }
    }
}
