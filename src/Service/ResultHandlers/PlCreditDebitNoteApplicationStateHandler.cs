namespace Linn.Purchasing.Service.ResultHandlers
{
    using Linn.Common.Facade.Carter.Handlers;
    using Linn.Purchasing.Resources;

    public class PlCreditDebitNoteApplicationStateResultHandler : JsonResultHandler<PlCreditDebitNoteResource>
    {
        public PlCreditDebitNoteApplicationStateResultHandler() : base("application/vnd.linn.application-state+json;version=1")
        {
        }
    }
}
