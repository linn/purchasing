namespace Linn.Purchasing.IoC
{
    using System;

    using Amazon.SQS;

    using Linn.Common.Logging;
    using Linn.Common.Logging.AmazonSqs;

    using Microsoft.Extensions.DependencyInjection;

    public static class LoggingExtensions
    {
        public static IServiceCollection AddLog(this IServiceCollection services)
        {
            return services.AddSingleton<ILog>(
                l =>
                    {
                        var sqs = l.GetRequiredService<IAmazonSQS>();
                        return new AmazonSqsLog(
                            sqs,
                            LoggingConfiguration.Environment,
                            LoggingConfiguration.MaxInnerExceptionDepth,
                            LoggingConfiguration.AmazonSqsQueueUri);
                    });
        }
    }
}
