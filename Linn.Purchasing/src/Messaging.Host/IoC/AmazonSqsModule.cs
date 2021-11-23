namespace Linn.Purchasing.Messaging.Host.IoC
{
    using Amazon;
    using Amazon.Runtime;
    using Amazon.SQS;

    using Autofac;

    public class AmazonSqsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AmazonSQSClient>()
                .As<IAmazonSQS>()
                .UsingConstructor(typeof(AWSCredentials), typeof(RegionEndpoint))
                .SingleInstance();
        }
    }
}
