using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Infrastructure.Data.Mappings;

public class StockMovementMapping : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("StockMovements");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MovementType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.PreviousQuantity)
            .IsRequired();

        builder.Property(x => x.NewQuantity)
            .IsRequired();

        builder.Property(x => x.Reason)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.StockMovements)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany(x => x.StockMovements)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => x.IsActive);
    }
}