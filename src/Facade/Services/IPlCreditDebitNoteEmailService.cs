namespace Linn.Purchasing.Facade.Services
{
    using System.IO;

    using Linn.Common.Facade;
    using Linn.Common.Resources;

    public interface IPlCreditDebitNoteEmailService
    {
        IResult<ProcessResultResource> SendEmail(int senderUserNumber, int noteNumber, Stream attachment);
    }
}
