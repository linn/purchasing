namespace Linn.Purchasing.Domain.LinnApps
{
    using Linn.Common.Email;
    using Linn.Common.Pdf;
    using Linn.Purchasing.Domain.LinnApps.Dispatchers;

    public class ThingService : IThingService
    {
        private readonly IMessageSender sender;

        private readonly IEmailService emailSender;

        private readonly IPdfService pdfService;

        private readonly ITemplateEngine templateEngine;

        public ThingService(
            IMessageSender sender, 
            IEmailService emailSender, 
            IPdfService pdfService, 
            ITemplateEngine templateEngine)
        {
            this.sender = sender;
            this.emailSender = emailSender;
            this.pdfService = pdfService;
            this.templateEngine = templateEngine;
        }

        public void SendThingMessage(string message)
        {
            this.sender.SendMessage(message);
        }

        public Thing CreateThing(Thing thing)
        {
            var html = 
                this.templateEngine.Render(thing, "<html><h1>Thing Name: {{ name }}</h1></html>")
                .Result;

            this.emailSender.SendEmail(
                thing.RecipientAddress,
                thing.RecipientName,
                null,
                null,
                "things@linn.co.uk",
                "Linn Things",
                thing.Name,
                thing.CodeId.ToString(),
                this.pdfService.ConvertHtmlToPdf(html, false).Result,
                "attachment.pdf");

            return thing;
        }
    }
}
