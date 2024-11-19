using FastCarSales.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;


namespace FastCarSales.Data
{
    public class FastCarSalesDbContext : IdentityDbContext
    {
        public FastCarSalesDbContext(DbContextOptions<FastCarSalesDbContext> options) : base(options)
        {
        }

        public DbSet<BodyType> BodyTypes { get; set; }
        public DbSet<Car> Cars { get; set; }

        public DbSet<CarExtra> CarExtras { get; set; }

        public DbSet<Extra> Extras { get; set; }
        public DbSet<ExtraType> ExtraTypes { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Post> Posts { get; set; }

        
        public DbSet<TransmissionType> TransmissionTypes { get; set; }

       

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Car>()
                .HasOne(c => c.BodyType)
                .WithMany(c => c.Cars)
                .HasForeignKey(c => c.BodyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Car>()
                .HasOne(c => c.FuelType)
                .WithMany(ft => ft.Cars)
                .HasForeignKey(c => c.FuelTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
               .Entity<Car>()
               .HasOne(c => c.Make)
               .WithMany(c => c.Cars)
               .HasForeignKey(c => c.MakeId)
               .OnDelete(DeleteBehavior.Restrict);

            builder
               .Entity<Car>()
               .HasOne(c => c.CarModel)
               .WithMany(c => c.Cars)
               .HasForeignKey(c => c.CarModelId)
               .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Car>()
                .HasOne(c => c.TransmissionType)
                .WithMany(tt => tt.Cars)
                .HasForeignKey(c => c.TransmissionTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Car>()
                .HasOne(c => c.Post)
                .WithOne(p => p.Car)
                .HasForeignKey<Post>(p => p.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Car>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18,2)");
                    
            builder.Entity<CarModel>()                
                .HasKey(e => e.Id);

            builder.Entity<CarModel>()                              
                .Property(e => e.Name) 
                .IsRequired() 
                .HasMaxLength(50);

            builder.Entity<CarModel>()                
                .HasOne(e => e.Make) 
                .WithMany(m => m.CarModels) 
                .HasForeignKey(e => e.MakeId) 
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CarModel>()
                .HasOne(e => e.BodyType) 
                .WithMany(b => b.CarModels) 
                .HasForeignKey(e => e.BodyTypeId) 
                .OnDelete(DeleteBehavior.Restrict);

			builder.Entity<CarModel>()
				.HasOne(e => e.FuelType)
				.WithMany(b => b.CarModels)
				.HasForeignKey(e => e.FuelTypeId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<CarModel>()
				.HasOne(e => e.TransmissionType)
				.WithMany(b => b.CarModels)
				.HasForeignKey(e => e.TransmissionTypeId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<Make>(entity => 
            { 
                entity.HasKey(e => e.Id); 
                entity.Property(e => e.Name)
                .IsRequired().HasMaxLength(50); }); 

            builder
                .Entity<BodyType>()
                .HasKey(e => e.Id);

            builder
               .Entity<BodyType>()
               .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
           
                builder
                .Entity<Post>()
                .HasOne(p => p.Creator)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<ApplicationUser>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.Creator)
                .HasForeignKey(p => p.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Extra>()
                .HasOne(e => e.Type)
                .WithMany(et => et.Extras)
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<CarExtra>()
                .HasKey(ce => new { ce.CarId, ce.ExtraId });

            builder
                .Entity<CarExtra>()
                .HasOne(ce => ce.Car)
                .WithMany(c => c.CarExtras)
                .HasForeignKey(ce => ce.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<CarExtra>()
                .HasOne(ce => ce.Extra)
                .WithMany(e => e.CarExtras)
                .HasForeignKey(ce => ce.ExtraId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);

            //builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "User", NormalizedName = "USER", Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString() });
            //builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN", Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString() });
        }
    }
}
