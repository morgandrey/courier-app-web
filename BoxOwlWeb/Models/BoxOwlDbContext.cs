using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BoxOwlWeb.Models
{
    public partial class BoxOwlDbContext : DbContext
    {
        public BoxOwlDbContext()
        {
        }

        public BoxOwlDbContext(DbContextOptions<BoxOwlDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Courier> Courier { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductOrder> ProductOrder { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;Database=BoxOwlDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.ClientId)
                    .HasName("Client_pk")
                    .IsClustered(false);

                entity.Property(e => e.ClientId).HasColumnName("clientId");

                entity.Property(e => e.ClientEmail)
                    .IsRequired()
                    .HasColumnName("clientEmail")
                    .HasMaxLength(100);

                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasColumnName("clientName")
                    .HasMaxLength(150);

                entity.Property(e => e.ClientPassword)
                    .IsRequired()
                    .HasColumnName("clientPassword")
                    .HasMaxLength(100);

                entity.Property(e => e.ClientPhone)
                    .IsRequired()
                    .HasColumnName("clientPhone")
                    .HasMaxLength(16);

                entity.Property(e => e.ClientSalt)
                    .IsRequired()
                    .HasColumnName("clientSalt")
                    .HasMaxLength(100);

                entity.Property(e => e.ClientSurname)
                    .IsRequired()
                    .HasColumnName("clientSurname")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Courier>(entity =>
            {
                entity.HasKey(e => e.CourierId)
                    .HasName("Courier_pk")
                    .IsClustered(false);

                entity.Property(e => e.CourierId).HasColumnName("courierId");

                entity.Property(e => e.CourierImage).HasColumnName("courierImage");

                entity.Property(e => e.CourierMoney).HasColumnName("courierMoney");

                entity.Property(e => e.CourierName)
                    .IsRequired()
                    .HasColumnName("courierName")
                    .HasMaxLength(150);

                entity.Property(e => e.CourierPassword)
                    .IsRequired()
                    .HasColumnName("courierPassword")
                    .HasMaxLength(50);

                entity.Property(e => e.CourierPhone)
                    .IsRequired()
                    .HasColumnName("courierPhone")
                    .HasMaxLength(25);

                entity.Property(e => e.CourierRating).HasColumnName("courierRating");

                entity.Property(e => e.CourierSalt)
                    .IsRequired()
                    .HasColumnName("courierSalt")
                    .HasMaxLength(100);

                entity.Property(e => e.CourierSurname)
                    .IsRequired()
                    .HasColumnName("courierSurname")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("Order_pk")
                    .IsClustered(false);

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.ClientId).HasColumnName("clientId");

                entity.Property(e => e.CourierId).HasColumnName("courierId");

                entity.Property(e => e.CourierReward).HasColumnName("courierReward");

                entity.Property(e => e.DeliveryAddress)
                    .IsRequired()
                    .HasColumnName("deliveryAddress");

                entity.Property(e => e.OrderCost).HasColumnName("orderCost");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("orderDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderDescription).HasColumnName("orderDescription");

                entity.Property(e => e.OrderRating).HasColumnName("orderRating");

                entity.Property(e => e.OrderStatusId).HasColumnName("orderStatusId");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("Order_Client_clientId_fk");

                entity.HasOne(d => d.Courier)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.CourierId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Order_Courier_courierId_fk");

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.OrderStatusId)
                    .HasConstraintName("Order_OrderStatus_idOrderStatus_fk");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasKey(e => e.IdOrderStatus)
                    .HasName("OrderStatus_pk")
                    .IsClustered(false);

                entity.Property(e => e.IdOrderStatus).HasColumnName("idOrderStatus");

                entity.Property(e => e.OrderStatusName)
                    .IsRequired()
                    .HasColumnName("orderStatusName")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("Product_pk")
                    .IsClustered(false);

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.ProductCost).HasColumnName("productCost");

                entity.Property(e => e.ProductDescription).HasColumnName("productDescription");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnName("productName")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ProductOrder>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("ProductOrder_pk")
                    .IsClustered(false);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ProductOrder)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("ProductOrder_Order_orderId_fk");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductOrder)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("ProductOrder_Product_productId_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
