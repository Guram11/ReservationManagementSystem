using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Identity.Models;

namespace ReservationManagementSystem.Infrastructure.Context;

public class DataContext : IdentityDbContext<ApplicationUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<AvailabilityTimeline> AvailabilityTimelines { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<HotelService> HotelServices { get; set; }
    public DbSet<Rate> Rates { get; set; }
    public DbSet<RateRoomType> RateRoomTypes { get; set; }
    public DbSet<RateTimeline> RateTimelines { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<ReservationInvoices> ReservationInvoices { get; set; }
    public DbSet<ReservationRoom> ReservationRooms { get; set; }
    public DbSet<ReservationRoomPayments> ReservationRoomPayments { get; set; }
    public DbSet<ReservationRoomServices> ReservationRoomServices { get; set; }
    public DbSet<ReservationRoomTimeline> ReservationRoomTimelines { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<CurrencyRate> CurrencyRates { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuring composite keys
        modelBuilder.Entity<RateRoomType>()
            .HasKey(rt => new { rt.RateId, rt.RoomTypeId });

        modelBuilder.Entity<ReservationRoomServices>()
            .HasKey(rrs => new { rrs.HotelServiceId, rrs.ReservationRoomId });

        // Configuring relationships with schema
        modelBuilder.Entity<AvailabilityTimeline>()
            .HasOne(at => at.RoomType)
            .WithMany(rt => rt.AvailabilityTimelines)
            .HasForeignKey(at => at.RoomTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Guest>()
            .HasOne(g => g.ReservationRoom)
            .WithMany(rr => rr.Guests)
            .HasForeignKey(g => g.ReservationRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Hotel>()
            .HasMany(h => h.RoomTypes)
            .WithOne(rt => rt.Hotel)
            .HasForeignKey(rt => rt.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Hotel>()
            .HasMany(h => h.Services)
            .WithOne(hs => hs.Hotel)
            .HasForeignKey(hs => hs.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HotelService>()
            .HasOne(hs => hs.Hotel)
            .WithMany(h => h.Services)
            .HasForeignKey(hs => hs.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Rate>()
            .HasMany(r => r.RateRoomTypes)
            .WithOne(rrt => rrt.Rate)
            .HasForeignKey(rrt => rrt.RateId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RateRoomType>()
            .HasOne(rrt => rrt.RoomType)
            .WithMany(rt => rt.RateRoomTypes)
            .HasForeignKey(rrt => rrt.RoomTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RateRoomType>()
            .HasMany(rrt => rrt.RateTimelines)
            .WithOne(rt => rt.RateRoomType)
            .HasForeignKey(rt => new { rt.RateId, rt.RoomTypeId })
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reservation>()
            .HasMany(r => r.ReservationRooms)
            .WithOne(rr => rr.Reservation)
            .HasForeignKey(rr => rr.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reservation>()
            .HasMany(r => r.Invoices)
            .WithOne(i => i.Reservation)
            .HasForeignKey(i => i.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReservationRoom>()
            .HasMany(rr => rr.ReservationRoomTimelines)
            .WithOne(t => t.ReservationRoom)
            .HasForeignKey(t => t.ReservationRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReservationRoom>()
            .HasMany(rr => rr.ReservationRoomServices)
            .WithOne(rs => rs.ReservationRoom)
            .HasForeignKey(rs => rs.ReservationRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReservationRoom>()
            .HasMany(rr => rr.ReservationRoomPayments)
            .WithOne(p => p.ReservationRoom)
            .HasForeignKey(p => p.ReservationRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReservationRoom>()
            .HasOne(rr => rr.RateRoomType)
            .WithMany(r => r.ReservationRooms)
            .HasForeignKey(rr => new { rr.RateId, rr.RoomTypeId })
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReservationRoom>()
            .HasOne(rr => rr.Room)
            .WithMany(r => r.ReservationRooms)
            .HasForeignKey(rr => rr.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReservationRoomServices>()
            .HasOne(rrs => rrs.HotelService)
            .WithMany(hs => hs.ReservationRoomServices)
            .HasForeignKey(rrs => rrs.HotelServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReservationRoomServices>()
            .HasOne(rrs => rrs.ReservationRoom)
            .WithMany(rr => rr.ReservationRoomServices)
            .HasForeignKey(rrs => rrs.ReservationRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Room>()
            .HasOne(r => r.RoomType)
            .WithMany(rt => rt.Rooms)
            .HasForeignKey(r => r.RoomTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}