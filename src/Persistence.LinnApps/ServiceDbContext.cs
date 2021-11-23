namespace Linn.Purchasing.Persistence.LinnApps
{
    using Linn.Common.Configuration;
    using Linn.Purchasing.Domain.LinnApps;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class ServiceDbContext : DbContext
    {
        public static readonly LoggerFactory MyLoggerFactory =
            new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });

        public DbSet<Thing> Things { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Model.AddAnnotation("MaxIdentifierLength", 30);
            this.BuildThings(builder);
            this.BuildThingDetails(builder);
            this.BuildThingCodes(builder);
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var host = ConfigurationManager.Configuration["DATABASE_HOST"];
            var userId = ConfigurationManager.Configuration["DATABASE_USER_ID"];
            var password = ConfigurationManager.Configuration["DATABASE_PASSWORD"];
            var serviceId = ConfigurationManager.Configuration["DATABASE_NAME"];

            var dataSource =
                $"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={serviceId})(SERVER=dedicated)))";

            var connectionString = $"Data Source={dataSource};User Id={userId};Password={password};";

            optionsBuilder.UseOracle(connectionString, options => options.UseOracleSQLCompatibility("11"));
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            optionsBuilder.EnableSensitiveDataLogging(true);
            base.OnConfiguring(optionsBuilder);
        }

        private void BuildThings(ModelBuilder builder)
        {
            var table = builder.Entity<Thing>().ToTable("THINGS");
            table.HasKey(a => a.Id);
            table.Property(a => a.Id).HasColumnName("ID");
            table.Property(a => a.Name).HasColumnName("NAME");
            table.Property(a => a.CodeId).HasColumnName("THING_CODE");
            table.Property(a => a.RecipientAddress).HasColumnName("RECIPIENT_ADDRESS");
            table.Property(a => a.RecipientName).HasColumnName("RECIPIENT_NAME");
            table.HasOne(d => d.Code).WithMany(p => p.Things).HasForeignKey(d => d.CodeId);
            table.HasMany(a => a.Details).WithOne();
        }

        private void BuildThingDetails(ModelBuilder builder)
        {
            var h = builder.Entity<ThingDetail>().ToTable("THING_DETAILS");
            h.HasKey(a => new { a.ThingId, a.DetailId });
            h.Property(a => a.ThingId).HasColumnName("THING_ID");
            h.Property(a => a.DetailId).HasColumnName("DETAIL_ID");
            h.Property(a => a.Description).HasColumnName("DESCRIPTION");
        }

        private void BuildThingCodes(ModelBuilder builder)
        {
            var h = builder.Entity<ThingCode>().ToTable("THING_CODES");
            h.HasKey(a => a.Code);
            h.Property(a => a.Code).HasColumnName("CODE");
            h.Property(a => a.CodeName).HasColumnName("CODE_NAME");
        }
    }
}
