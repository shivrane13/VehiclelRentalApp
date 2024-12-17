using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using VehiclelRentalApp.Areas.Identity.Data;
using VehiclelRentalApp.Models;

namespace VehiclelRentalApp.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<RentalBooking> RentalBookings { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<RentalBooking>()
        .HasOne(rb => rb.User)
        .WithMany()
        .HasForeignKey(rb => rb.userId);

        builder.Entity<RentalBooking>(entity =>
        {
            entity.Property(e => e.TotalCost).HasPrecision(18, 2);
        });

        builder.Entity<Vehicle>(entity =>
        {
            entity.Property(e => e.DailyRate).HasPrecision(18, 2);
            entity.Property(e => e.HourlyRate).HasPrecision(18, 2);
        });
    }
}
