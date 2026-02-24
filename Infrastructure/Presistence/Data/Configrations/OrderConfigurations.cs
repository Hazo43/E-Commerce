using Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Data.Configrations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Owend ShippingAddress
            builder.OwnsOne(X => X.ShippingAddress, OwndEntity =>
            {
                OwndEntity.Property(X => X.FirstName).HasMaxLength(50);
                OwndEntity.Property(X => X.LastName).HasMaxLength(50);
                OwndEntity.Property(X => X.City).HasMaxLength(50);
                OwndEntity.Property(X => X.Street).HasMaxLength(50);
                OwndEntity.Property(X => X.Country).HasMaxLength(50);
            });

            // Relations With OrderItems
            builder.HasMany(x => x.OrderItems)
                   .WithOne();

            // Relations With DeliveryMethod 
            builder.HasOne(x => x.DeliveryMethod)
                   .WithMany()
                   .HasForeignKey(x => x.DeliverMethodId)
                   .OnDelete(DeleteBehavior.SetNull);


            // OrderPaymentStatus
            // عادي Enum لاكن وانت راجع هترجعها ع شكل ال String خزنها DB كدا بقولو و انت ؤايح تخزنها في ال
            builder.Property(x => x.OrderPaymentStatus).HasConversion(
                            ps => ps.ToString(), ps => Enum.Parse<OrderPaymentStatus>(ps));

            // SubTotal 
            builder.Property(s => s.SubTotal).HasColumnType("decimal(18,4)");
        }
    }
}
