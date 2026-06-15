using Microsoft.EntityFrameworkCore;
using VTBL.RabbitIntegration.Monitor.Context.Entities;

namespace VTBL.RabbitIntegration.Monitor.Context
{
    /// <summary>
    /// EF Core контекст для базы <c>MSCRM_EXT</c> с таблицами интеграционных сообщений и справочников.
    /// </summary>
    public class MscrmExtDbContext : DbContext
    {
        /// <summary>
        /// Создаёт контекст с параметрами подключения к БД.
        /// </summary>
        /// <param name="options">Параметры EF Core контекста.</param>
        public MscrmExtDbContext(DbContextOptions<MscrmExtDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Сообщения интеграции ККА.
        /// </summary>
        public DbSet<ExportMessageRabbitKka> ExportMessageRabbitKkas { get; set; } = null!;

        /// <summary>
        /// Статусы сообщений интеграции ККА.
        /// </summary>
        public DbSet<ExportMessageRabbitKkaStatus> ExportMessageRabbitKkaStatuses { get; set; } = null!;

        /// <summary>
        /// Сообщения интеграции NOVA.
        /// </summary>
        public DbSet<ExportMessageRabbitNova> ExportMessageRabbitNovas { get; set; } = null!;

        /// <summary>
        /// Статусы сообщений интеграции NOVA.
        /// </summary>
        public DbSet<ExportMessageRabbitNovaStatus> ExportMessageRabbitNovaStatuses { get; set; } = null!;

        /// <summary>
        /// Сообщения интеграции Remarketing.
        /// </summary>
        public DbSet<ExportMessageRabbitRemarketing> ExportMessageRabbitRemarketings { get; set; } = null!;

        /// <summary>
        /// Статусы сообщений интеграции Remarketing.
        /// </summary>
        public DbSet<ExportMessageRabbitRemarketingStatus> ExportMessageRabbitRemarketingStatuses { get; set; } = null!;

        /// <summary>
        /// Справочник наименований статусов.
        /// </summary>
        public DbSet<ExportMessageRabbitStatusName> ExportMessageRabbitStatusNames { get; set; } = null!;

        /// <summary>
        /// Конфигурации ключей операций Rabbit.
        /// </summary>
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

                // Логическая связь OperationKey -> RabbitIntegrationOperationKeysConfiguration.Key без FK в БД.
                entity.HasOne(e => e.OperationConfiguration)
                    .WithMany(c => c.ExportMessages)
                    .HasForeignKey(e => e.OperationKey)
                    .HasPrincipalKey(c => c.Key)
                    .OnDelete(DeleteBehavior.NoAction);
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

                // Логическая связь IntegrationId -> ExportMessageRabbitKKA.Id без FK в БД.
                entity.HasOne(e => e.Integration)
                    .WithMany(k => k.StatusHistory)
                    .HasForeignKey(e => e.IntegrationId)
                    .HasPrincipalKey(k => k.Id)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ExportMessageRabbitNova>(entity =>
            {
                entity.ToTable("ExportMessageRabbitNOVA");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.OperationKey).HasMaxLength(100).IsRequired();

                entity.Property(e => e.Endpoint).HasMaxLength(100).IsRequired();

                entity.Property(e => e.Body).HasColumnType("nvarchar(max)");

                // Логическая связь OperationKey -> RabbitIntegrationOperationKeysConfiguration.Key без FK в БД.
                entity.HasOne(e => e.OperationConfiguration)
                    .WithMany(c => c.NovaExportMessages)
                    .HasForeignKey(e => e.OperationKey)
                    .HasPrincipalKey(c => c.Key)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ExportMessageRabbitNovaStatus>(entity =>
            {
                entity.ToTable("ExportMessageRabbitNOVAStatus");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasDefaultValueSql("newid()").ValueGeneratedOnAdd();

                entity.Property(e => e.Created).HasColumnType("datetime").HasDefaultValueSql("getdate()");

                entity.Property(e => e.ErrorMessage).HasColumnType("nvarchar(max)");

                entity.Property(e => e.SendMessage).HasColumnType("nvarchar(max)");

                entity.Property(e => e.RowVersion).IsRowVersion();

                // Логическая связь IntegrationId -> ExportMessageRabbitNOVA.Id без FK в БД.
                entity.HasOne(e => e.Integration)
                    .WithMany(k => k.StatusHistory)
                    .HasForeignKey(e => e.IntegrationId)
                    .HasPrincipalKey(k => k.Id)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ExportMessageRabbitRemarketing>(entity =>
            {
                entity.ToTable("ExportMessageRabbitREMARKETING");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.OperationKey).HasMaxLength(100).IsRequired();

                entity.Property(e => e.Endpoint).HasMaxLength(100).IsRequired();

                entity.Property(e => e.Body).HasColumnType("nvarchar(max)");

                // Логическая связь OperationKey -> RabbitIntegrationOperationKeysConfiguration.Key без FK в БД.
                entity.HasOne(e => e.OperationConfiguration)
                    .WithMany(c => c.RemarketingExportMessages)
                    .HasForeignKey(e => e.OperationKey)
                    .HasPrincipalKey(c => c.Key)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ExportMessageRabbitRemarketingStatus>(entity =>
            {
                entity.ToTable("ExportMessageRabbitREMARKETINGStatus");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasDefaultValueSql("newid()").ValueGeneratedOnAdd();

                entity.Property(e => e.Created).HasColumnType("datetime").HasDefaultValueSql("getdate()");

                entity.Property(e => e.ErrorMessage).HasColumnType("nvarchar(max)");

                entity.Property(e => e.SendMessage).HasColumnType("nvarchar(max)");

                entity.Property(e => e.RowVersion).IsRowVersion();

                // Логическая связь IntegrationId -> ExportMessageRabbitREMARKETING.Id без FK в БД.
                entity.HasOne(e => e.Integration)
                    .WithMany(k => k.StatusHistory)
                    .HasForeignKey(e => e.IntegrationId)
                    .HasPrincipalKey(k => k.Id)
                    .OnDelete(DeleteBehavior.NoAction);
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

                entity.HasAlternateKey(e => e.Key);

                entity.Property(e => e.PackageBuilderURL).HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.SendPackageRabbitExchange).HasColumnType("nvarchar(max)").IsRequired();

                entity.Property(e => e.SendPackageRabbitRoutingkey).HasColumnType("nvarchar(max)").IsRequired();
            });
        }
    }
}
