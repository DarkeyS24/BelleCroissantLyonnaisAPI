using BelleCroissantLyonnaisAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BelleCroissantLyonnaisAPI.AppContext
{
    public class AppContextDB : DbContext
    {
        public AppContextDB(DbContextOptions<AppContextDB> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Payment_Method> Payment_Methods { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(order =>
            {
                order.HasKey(o => o.order_id).HasName("order_id");
                order.HasOne(o => o.customer)
                     .WithMany()
                     .HasForeignKey(o => o.customer_id).HasConstraintName("customer_id");

                order.HasOne(o => o.product)
                     .WithMany()
                     .HasForeignKey(o => o.product_id).HasConstraintName("product_id");

                order.HasOne(o => o.channel)
                     .WithMany()
                     .HasForeignKey(o => o.channel_id).HasConstraintName("channel_id");

                order.HasOne(o => o.payment)
                     .WithMany()
                     .HasForeignKey(o => o.payment_method_id).HasConstraintName("payment_method_id");
            });
            modelBuilder.Entity<Customer>(custom =>
            {
                custom.HasKey(c => c.customer_id).HasName("customer_id");
                custom.Property(c => c.age).HasColumnType("tinyint").IsRequired();
                custom.HasOne(c => c.membership)
                .WithMany()
                .HasForeignKey(c => c.membership_id).HasConstraintName("membership_id");
                custom.Property(c => c.churned).HasColumnType("bit").IsRequired();
            });
            modelBuilder.Entity<Product>(product =>
            {
                product.HasKey(p => p.product_id).HasName("product_id");
                product.HasOne(p => p.category)
                       .WithMany()
                       .HasForeignKey(p => p.category_id).HasConstraintName("category_id");
                product.Property(p => p.seasonal).HasColumnType("bit").IsRequired();
                product.Property(p => p.active).HasColumnType("bit").IsRequired();
            });
            modelBuilder.Entity<Channel>(channel =>
            {
                channel.HasKey(c => c.channel_id).HasName("channel_id");
            });
            modelBuilder.Entity<Membership>(membership =>
            {
                membership.HasKey(m => m.membership_id).HasName("membership_id");
            });
            modelBuilder.Entity<Payment_Method>(payment =>
            {
                payment.HasKey(p => p.payment_method_id).HasName("payment_method_id");
            });
            modelBuilder.Entity<Category>(category =>
            {
                category.HasKey(c => c.category_id).HasName("category_id");
            });
        }
    }
}
