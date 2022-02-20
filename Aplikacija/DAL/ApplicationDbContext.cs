using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DAL.Models;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public virtual DbSet<Food> Food { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasMany(x => x.Orders).WithOne(x => x.Client).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<User>().HasMany(x => x.OrdersToDeliver).WithOne(x => x.Deliverer).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>().HasMany(x => x.Breakfast)
                .WithMany(x => x.BreakfastOrders);

            builder.Entity<Order>().HasMany(x => x.Lunch)
                .WithMany(x => x.LunchOrders);

            builder.Entity<Order>().HasMany(x => x.Dinner)
                .WithMany(x => x.DinnerOrders);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<Food>().ToTable("Food");
            builder.Entity<Menu>().ToTable("Menus");
            builder.Entity<Order>().ToTable("Orders");
            builder.Entity<Comment>().ToTable("Comments");
            base.OnModelCreating(builder);
        }
    }
}