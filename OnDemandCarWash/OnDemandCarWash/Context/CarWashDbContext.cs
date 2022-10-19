using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OnDemandCarWash.Models;
using System.Reflection.Metadata;


namespace OnDemandCarWash.Context
{
    public class CarWashDbContext:DbContext
    {
        public CarWashDbContext(DbContextOptions<CarWashDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CarDetail> carDetails { get; set; }
        public DbSet<AfterWash> afterWashes { get; set; }
        public DbSet<WashType> washTypes { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<Promocode> Promocodes { get; set; }
        public DbSet<Admin> Admins { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // user -- address relation one to one
            modelBuilder.Entity<Address>().HasKey(a => a.userId);
            modelBuilder.Entity<User>()
                .HasOne(p => p.Address)
                .WithOne(b => b.User);

            //order -- cardetail relation one to one
            modelBuilder.Entity<CarDetail>().HasKey(c => c.orderId);
            modelBuilder.Entity<Order>()
                .HasOne(p => p.CarDetail)
                .WithOne(b => b.Orders);

            //order -- cardetail relation one to one
            modelBuilder.Entity<AfterWash>().HasKey(c => c.orderId);
            modelBuilder.Entity<Order>()
                .HasOne(p => p.AfterWash)
                .WithOne(b => b.Orders);

            // order -- washType relation many to one
            modelBuilder.Entity<WashType>().HasKey(c => c.washTypeId);
            modelBuilder.Entity<Order>()
               .HasOne(p => p.WashTypes)
               .WithMany(b => b.Orders);

            // order -- paymentDetail relation one to many
            modelBuilder.Entity<PaymentDetail>().HasKey(c => c.paymentId);
            modelBuilder.Entity<PaymentDetail>()
               .HasOne(p => p.Orders)
               .WithMany(b => b.PaymentDetails);

            // paymentDetails -- promocode relation many to one
            modelBuilder.Entity<Promocode>().HasKey(c => c.promoId);
            modelBuilder.Entity<PaymentDetail>()
               .HasOne(p => p.Promocodes)
               .WithMany(b => b.PaymentDetails);

            // order -- user relation many to one
            modelBuilder.Entity<Order>()
               .HasOne(p => p.User)
               .WithMany(b => b.Orders);
        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    // Configure Student & StudentAddress entity
        //    modelBuilder.Entity<User>()
        //                .HasOptional(s => s.Address) // Mark Address property optional in Student entity
        //                .WithRequired(ad => ad.Student); // mark Student property as required in StudentAddress entity. Cannot save StudentAddress without Student
        //}
    }
}
