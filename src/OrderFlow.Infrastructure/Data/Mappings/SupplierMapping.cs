using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Infrastructure.Data.Mappings;

public class SupplierMapping : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Document)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(x => x.Document)
            .IsUnique();

        builder.Property(x => x.ContactName)
            .HasMaxLength(150);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Address)
            .HasMaxLength(200);

        builder.Property(x => x.City)
            .HasMaxLength(200);

        builder.Property(x => x.State)
            .HasMaxLength(2);

        builder.Property(x => x.ZipCode)
            .HasMaxLength(10);

        builder.HasMany(x => x.Products)
            .WithOne(x => x.Supplier)
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => x.IsActive);
    }
}