namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class PlCreditDebitNoteFacadeService 
        : FacadeFilterResourceService<PlCreditDebitNote, int, PlCreditDebitNoteResource, PlCreditDebitNoteResource, PlCreditDebitNoteResource>
    {
        private readonly IPlCreditDebitNoteService domainService;

        public PlCreditDebitNoteFacadeService(
            IRepository<PlCreditDebitNote, int> repository, 
            ITransactionManager transactionManager, 
            IBuilder<PlCreditDebitNote> builder,
            IPlCreditDebitNoteService domainService)
            : base(repository, transactionManager, builder)
        {
            this.domainService = domainService;
        }

        protected override PlCreditDebitNote CreateFromResource(
            PlCreditDebitNoteResource resource, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(
            PlCreditDebitNote entity,
            PlCreditDebitNoteResource updateResource,
            IEnumerable<string> privileges = null)
        {
            if (updateResource.Close.HasValue && (bool)updateResource.Close)
            {
                this.domainService.CloseDebitNote(entity, updateResource.ReasonClosed, privileges);
            }
        }

        protected override Expression<Func<PlCreditDebitNote, bool>> SearchExpression(
            string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            PlCreditDebitNote entity,
            PlCreditDebitNoteResource resource,
            PlCreditDebitNoteResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            PlCreditDebitNote entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<PlCreditDebitNote, bool>> FilterExpression(
            PlCreditDebitNoteResource searchResource)
        {
            var date = string.IsNullOrEmpty(searchResource.DateClosed)
                           ? (DateTime?)null
                           : DateTime.Parse(searchResource.DateClosed);
            return x => x.DateClosed == date;
        }
    }
}
