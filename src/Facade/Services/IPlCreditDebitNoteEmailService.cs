namespace Linn.Purchasing.Facade.Services
{
    using System.IO;

    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;

    public interface IPlCreditDebitNoteEmailService
    {
        IResult<ProcessResultResource> SendEmail(int noteNumber, Stream attachment);
    }
}
