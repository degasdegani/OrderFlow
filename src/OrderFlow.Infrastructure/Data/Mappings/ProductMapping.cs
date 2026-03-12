using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Infrastructure.Data.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.SKU)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.SKU)
            .IsUnique();

        builder.Property(x => x.BarCode)
            .HasMaxLength(50);

        builder.Property(x => x.CostPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.SalePrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Unit)
            .IsRequired()
            .HasMaxLength(10)
            .HasDefaultValue("UN");

        builder.Ignore(x => x.IsLowStock);
        builder.Ignore(x => x.ProfitMargin);

        builder.HasQueryFilter(x => x.IsActive);
    }
}