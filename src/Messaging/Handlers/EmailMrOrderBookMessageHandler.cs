namespace Linn.Purchasing.Messaging.Handlers
{
    using Linn.Common.Logging;
    using Linn.Purchasing.Messaging.Messages;

    using Newtonsoft.Json;

    public class EmailMrOrderBookMessageHandler : Handler<EmailMrOrderBookMessage>
    {
        public EmailMrOrderBookMessageHandler(ILog logger)
            : base(logger)
        {
        }

        public override bool Handle(EmailMrOrderBookMessage message)
        {
            try
            {
                return true;
            }
            catch (JsonReaderException e)
            {
                this.Logger.Error(e.Message);
                return false;
            }
        }
    }
}
