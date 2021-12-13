﻿namespace Linn.Purchasing.IoC
{
    using Linn.Common.Persistence;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Persistence.LinnApps;
    using Linn.Purchasing.Persistence.LinnApps.Keys;
    using Linn.Purchasing.Persistence.LinnApps.Repositories;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            return services.AddScoped<ServiceDbContext>().AddTransient<DbContext>(a => a.GetService<ServiceDbContext>())
                .AddTransient<ITransactionManager, TransactionManager>()
                .AddTransient<IRepository<Thing, int>, ThingRepository>()
                .AddTransient<IRepository<SigningLimit, int>, SigningLimitRepository>()
                .AddTransient<IRepository<PartSupplier, PartSupplierKey>, PartSupplierRepository>()
                .AddTransient<IQueryRepository<Part>, PartRepository>()
                .AddTransient<IRepository<Supplier, int>, SupplierRepository>()
                .AddTransient<IRepository<Currency, string>, EntityFrameworkRepository<Currency, string>>(r 
                    => new EntityFrameworkRepository<Currency, string>(r.GetService<ServiceDbContext>()?.Currencies))
                .AddTransient<IRepository<OrderMethod, string>, EntityFrameworkRepository<OrderMethod, string>>(r
                    => new EntityFrameworkRepository<OrderMethod, string>(r.GetService<ServiceDbContext>()?.OrderMethods))
                .AddTransient<IQueryRepository<LinnDeliveryAddress>, LinnDeliveryAddressRepository>();
        }
    }
}
