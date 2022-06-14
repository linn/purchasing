namespace Linn.Purchasing.Domain.LinnApps.Edi
{
    using System.Collections.Generic;

    public interface IEdiOrderService
    {
        IEnumerable<EdiOrder> GetEdiOrders(int supplierId);

        ProcessResult SendEdiOrder(int supplierId, string altEmail, string additionalEmail, string additionalText, bool test);
    }
}

