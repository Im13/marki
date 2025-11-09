using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class OfflineOrderConfiguration : IEntityTypeConfiguration<OfflineOrder>
    {
        public void Configure(EntityTypeBuilder<OfflineOrder> builder)
        {
            builder.ToTable("OfflineOrders");

            builder.Property(o => o.ShippingFee).HasColumnType("decimal(18,2)");
            builder.Property(o => o.OrderDiscount).HasColumnType("decimal(18,2)");
            builder.Property(o => o.BankTransferedAmount).HasColumnType("decimal(18,2)");
            builder.Property(o => o.ExtraFee).HasColumnType("decimal(18,2)");
            builder.Property(o => o.Total).HasColumnType("decimal(18,2)");

            builder.HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.OrderStatus)
                .WithMany()
                .HasForeignKey(o => o.OrderStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Province)
                .WithMany()
                .HasForeignKey(o => o.ProvinceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.District)
                .WithMany()
                .HasForeignKey(o => o.DistrictId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Ward)
                .WithMany()
                .HasForeignKey(o => o.WardId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

