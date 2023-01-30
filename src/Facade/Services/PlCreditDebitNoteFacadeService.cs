namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
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
            var note = new PlCreditDebitNote
                           {
                               PartNumber = resource.PartNumber, 
                               OrderQty = resource.OrderQty, 
                               ReturnsOrderNumber = resource.ReturnsOrderNumber,
                               ReturnsOrderLine = resource.ReturnsOrderLine ?? 1,
                               OriginalOrderLine = resource.ReturnsOrderLine ?? 1,
                               NetTotal = resource.NetTotal,
                               Total = resource.Total,
                               OrderUnitPrice = resource.OrderUnitPrice,
                               OrderUnitOfMeasure = resource.OrderUnitOfMeasure,
                               VatTotal = resource.VatTotal,
                               Notes = resource.Notes,
                               Supplier = new Supplier { SupplierId = resource.SupplierId.GetValueOrDefault() },
                               SuppliersDesignation = resource.SuppliersDesignation,
                               PurchaseOrder = new PurchaseOrder
                                                   {
                                                       OrderNumber = resource.OriginalOrderNumber.GetValueOrDefault()
                                                   },
                               Currency = new Currency { Code = resource.Currency },
                               VatRate = resource.VatRate,
                               CreditOrReplace = resource.CreditOrReplace,
                               OriginalOrderNumber = resource.OriginalOrderNumber,
                               CreatedBy = resource.Who.GetValueOrDefault()
                           };
            return this.domainService.CreateCreditNote(note, privileges);
        }

        protected override void UpdateFromResource(
            PlCreditDebitNote entity,
            PlCreditDebitNoteResource updateResource,
            IEnumerable<string> privileges = null)
        {
            var enumerable = privileges?.ToList();
            if (updateResource.Who.HasValue && updateResource.Close.HasValue && (bool)updateResource.Close)
            {
                this.domainService.CloseDebitNote(
                    entity, 
                    updateResource.ReasonClosed, 
                    (int)updateResource.Who, 
                    enumerable);
                return;
            }

            if (updateResource.Who.HasValue && !string.IsNullOrEmpty(updateResource.ReasonCancelled))
            {
                this.domainService.CancelDebitNote(
                    entity,
                    updateResource.ReasonClosed,
                    (int)updateResource.Who,
                    enumerable);
                return;
            }

            this.domainService.UpdatePlCreditDebitNote(
                entity, 
                new PlCreditDebitNote { Notes = updateResource.Notes }, 
                enumerable);
            return;
        }

        protected override Expression<Func<PlCreditDebitNote, bool>> SearchExpression(
            string searchTerm)
        {
            return x => x.NoteNumber.ToString() == searchTerm
                        || x.PurchaseOrder.OrderNumber.ToString() == searchTerm
                        || x.ReturnsOrderNumber.ToString() == searchTerm
                        || x.Supplier.SupplierId.ToString() == searchTerm
                        || x.Supplier.Name.ToUpper().Contains(searchTerm.ToUpper());
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
            return x => x.DateClosed == date && x.NoteType.Type == searchResource.NoteType;
        }

        protected override Expression<Func<PlCreditDebitNote, bool>> FindExpression(PlCreditDebitNoteResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
