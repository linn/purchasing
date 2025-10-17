namespace Linn.Purchasing.IoC
{
    
    using Linn.Common.Logging;
    using Microsoft.Extensions.DependencyInjection;

    public static class LoggingExtensions
    {
        public static IServiceCollection AddLog(this IServiceCollection services)
        {
            return services.AddSingleton<ILog, Linn.Common.Logging.ConsoleLog>();
        }
    }
}
