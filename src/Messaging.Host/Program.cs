namespace Linn.Purchasing.Messaging.Host
{
    using System;

    using Autofac;

    using Linn.Common.Logging;

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var container = Configuration.BuildContainer();
                using (var scope = container.BeginLifetimeScope())
                {
                    var log = scope.Resolve<ILog>();
                    var listener = new Listener(scope, log);

                    while (true)
                    {
                        listener.Listen();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                Environment.Exit(1);
            }
        }
    }
}
