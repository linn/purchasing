namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.IO;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;

    public interface IPurchaseOrderReqFacadeService : IFacadeResourceFilterService<PurchaseOrderReq, int,
        PurchaseOrderReqResource, PurchaseOrderReqResource, PurchaseOrderReqSearchResource>
    {
        IResult<PurchaseOrderReqResource> Authorise(
            int reqNumber,
            IEnumerable<string> privileges,
            int currentUserNumber);

        IResult<PurchaseOrderReqResource> CreateMiniOrderFromReq(
            int reqNumber,
            IEnumerable<string> privileges,
            int currentUserNumber);

        IResult<PurchaseOrderReqResource> FinanceAuthorise(
            int reqNumber,
            IEnumerable<string> privileges,
            int currentUserNumber);

        IResult<ProcessResultResource> SendAuthorisationRequestEmail(
            int currentUserNumber,
            int toEmployeeNumber,
            int reqNumber);

        IResult<ProcessResultResource> SendEmail(
            int senderUserNumber,
            string toEmailAddress,
            int reqNumber,
            Stream attachment);

        IResult<ProcessResultResource> SendFinanceCheckRequestEmail(
            int currentUserNumber,
            int toEmployeeNumber,
            int reqNumber);
    }
}
