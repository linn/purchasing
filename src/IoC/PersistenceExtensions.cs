﻿namespace Linn.Purchasing.IoC
{
    using Linn.Common.Persistence;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
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
                .AddTransient<IRepository<PartSupplier, PartSupplierKey>, EntityFrameworkRepository<PartSupplier, PartSupplierKey>>(
                    r => new EntityFrameworkRepository<PartSupplier, PartSupplierKey>(r.GetService<ServiceDbContext>()?.PartSuppliers));

            // Could also be
            // .AddTransient<IRepository<Thing, int>, EntityFrameworkRepository<Thing, int>>(r => new EntityFrameworkRepository<Thing, int>(r.GetService<ServiceDbContext>()?.Things))
        }
    }
}
