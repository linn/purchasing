namespace Linn.Purchasing.Messaging.Host.IoC
{
    using Amazon;
    using Amazon.Runtime;

    using Autofac;

    using Linn.Common.Configuration;

    public class AmazonCredentialsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(FallbackCredentialsFactory.GetCredentials())
                .As<AWSCredentials>()
                .SingleInstance();
            
            builder.Register(c => RegionEndpoint.GetBySystemName(AwsCredentialsConfiguration.Region))
                .As<RegionEndpoint>()
                .SingleInstance();
        }
    }
}
