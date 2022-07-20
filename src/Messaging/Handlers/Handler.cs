namespace Linn.Purchasing.Messaging.Handlers // will move to common
{
    using Linn.Common.Logging;

    public abstract class Handler<T>
    {
        protected Handler(ILog logger)
        {
            this.Logger = logger;
        }

        protected ILog Logger { get; set; }

        public abstract bool Handle(T message);
    }
}
