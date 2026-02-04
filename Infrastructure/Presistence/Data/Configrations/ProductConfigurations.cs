using Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Data.Configrations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Brand
            builder.HasOne( p => p.ProductBrand)
                   .WithMany( p => p.Products)
                   .HasForeignKey( p => p.BrandId );
          
            // Type
            builder.HasOne( p => p.productType)
                   .WithMany( p => p.Products)
                   .HasForeignKey( p => p.TypeId );

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)");

            builder.HasKey(X => X.Id);

            builder.Property(x => x.Name)
                   .HasMaxLength(100);

            builder.Property(x => x.Description)
                   .HasMaxLength(500);

            builder.Property(x => x.PictureUrl)
                   .HasMaxLength(200);

            builder.Property(x => x.Price)
                   .HasPrecision(18, 2);

        }
    }
}
