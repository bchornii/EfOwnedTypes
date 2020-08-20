using EfOwnedTypes.Models.Customers;
using EfOwnedTypes.Models.Customers.Orders;
using EfOwnedTypes.Models.Products;
using EfOwnedTypes.Models.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace EfOwnedTypes.EntityConfigurations
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        internal const string OrdersList = "_orders";
        internal const string OrderProducts = "_orderProducts";

        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers", SchemaNames.Orders);

            builder.HasKey(b => b.Id);  // TODO: no conversion
            builder.Property(b => b.Id)
                .HasConversion(x => x.Value, x => new CustomerId(x));

            builder.Property("_welcomeEmailWasSent").HasColumnName("WelcomeEmailWasSent");
            builder.Property("_email").HasColumnName("Email");
            builder.Property("_name").HasColumnName("Name");

            builder.OwnsMany<Order>(OrdersList, x =>
            {
                x.ToTable("Orders", SchemaNames.Orders);

                x.HasKey(b => b.Id);
                x.Property<OrderId>(b => b.Id)
                    .HasConversion(x => x.Value, x => new OrderId(x));

                x.WithOwner().HasForeignKey("CustomerId");

                x.Property<bool>("_isRemoved").HasColumnName("IsRemoved");
                x.Property<DateTime>("_orderDate").HasColumnName("OrderDate");
                x.Property<DateTime?>("_orderChangeDate").HasColumnName("OrderChangeDate");
                x.Property("_status").HasColumnName("StatusId")
                    .HasConversion(new EnumToNumberConverter<OrderStatus, byte>());

                x.OwnsMany<OrderProduct>(OrderProducts, y =>
                {
                    y.ToTable("OrderProducts", SchemaNames.Orders);

                    y.HasKey("OrderId", "ProductId");
                    y.Property<OrderId>("OrderId")
                        .HasConversion(x => x.Value, x => new OrderId(x));
                    y.Property<ProductId>("ProductId")
                        .HasConversion(x => x.Value, x => new ProductId(x));

                    y.WithOwner().HasForeignKey("OrderId");

                    y.OwnsOne<MoneyValue>("Value", mv =>
                    {
                        mv.Property(x => x.Currency).HasColumnName("Currency");
                        mv.Property(x => x.Value).HasColumnName("Value");
                    });

                    y.OwnsOne<MoneyValue>("ValueInEUR", mv =>
                    {
                        mv.Property(p => p.Currency).HasColumnName("CurrencyEUR");
                        mv.Property(p => p.Value).HasColumnName("ValueInEUR");
                    });
                });

                x.OwnsOne<MoneyValue>("_value", y =>
                {
                    y.Property(p => p.Currency).HasColumnName("Currency");
                    y.Property(p => p.Value).HasColumnName("Value");
                });

                x.OwnsOne<MoneyValue>("_valueInEUR", y =>
                {
                    y.Property(p => p.Currency).HasColumnName("CurrencyEUR");
                    y.Property(p => p.Value).HasColumnName("ValueInEUR");
                });
            });
        }
    }
}
