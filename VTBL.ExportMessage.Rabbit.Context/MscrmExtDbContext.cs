using Microsoft.EntityFrameworkCore;
using VTBL.ExportMessage.Rabbit.Context.Entities;

namespace VTBL.ExportMessage.Rabbit.Context
{
    public class MscrmExtDbContext : DbContext
    {
        public MscrmExtDbContext(DbContextOptions<MscrmExtDbContext> options)
            : base(options)
        {
        }

        public DbSet<ExportMessageRabbitKka> ExportMessageRabbitKkas { get; set; } = null!;

        public DbSet<ExportMessageRabbitKkaStatus> ExportMessageRabbitKkaStatuses { get; set; } = null!;

        public DbSet<ExportMessageRabbitStatusName> ExportMessageRabbitStatusNames { get; set; } = null!;

        public DbSet<RabbitIntegrationOperationKeysConfiguration> RabbitIntegrationOperationKeysConfigurations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExportMessageRabbitKka>(entity =>
            {
                entity.ToTable("ExportMessageRabbitKKA");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.OperationKey).HasMaxLength(100).IsRequired();

                entity.Property(e => e.Endpoint).HasMaxLength(100).IsRequired();

                entity.Property(e => e.Body).HasColumnType("nvarchar(max)");
            });

            modelBuilder.Entity<ExportMessageRabbitKkaStatus>(entity =>
            {
                entity.ToTable("ExportMessageRabbitKKAStatus");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasDefaultValueSql("newid()").ValueGeneratedOnAdd();

                entity.Property(e => e.Created).HasColumnType("datetime").HasDefaultValueSql("getdate()");

                entity.Property(e => e.ErrorMessage).HasColumnType("nvarchar(max)");

                entity.Property(e => e.SendMessage).HasColumnType("nvarchar(max)");

                entity.Property(e => e.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<ExportMessageRabbitStatusName>(entity =>
            {
                entity.ToTable("ExportMessageRabbitStatusName");

                entity.HasNoKey();

                entity.Property(e => e.StatusName).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<RabbitIntegrationOperationKeysConfiguration>(entity =>
            {
                entity.ToTable("RabbitIntegrationOperationKeysConfiguration");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasDefaultValueSql("newid()").ValueGeneratedOnAdd();

                entity.Property(e => e.Key).HasColumnName("Key").HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.PackageBuilderURL).HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.SendPackageRabbitExchange).HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.SendPackageRabbitRoutingkey).HasColumnType("nvarchar(max)").IsRequired();
            });
        }
    }
}
