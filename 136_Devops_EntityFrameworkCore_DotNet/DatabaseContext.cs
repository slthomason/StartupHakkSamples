using Microsoft.EntityFrameworkCore;

namespace ArticleDevOpsEfCore.Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProductEntity>(cfg =>
        {
            cfg.ToTable("Products");

            cfg.HasKey(e => e.Id);

            cfg.HasIndex(e => e.Code).IsUnique();

            cfg.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            cfg.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(8);
            cfg.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(128);
        });

        modelBuilder.Entity<StockMovementEntity>(cfg =>
        {
            cfg.ToTable("StockMovements");

            cfg.HasKey(e => e.Id);

            cfg.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            cfg.HasOne<ProductEntity>()
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .IsRequired();
            cfg.Property(e => e.Quantity)
                .IsRequired();
            cfg.Property(e => e.OccurredOn)
                .IsRequired();
        });
    }
}