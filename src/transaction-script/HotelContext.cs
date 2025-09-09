using Microsoft.EntityFrameworkCore;
using transaction_script.Models;

namespace transaction_script;

public class HotelContext(DbContextOptions<HotelContext> options) : DbContext(options)
{
    public DbSet<Room> Rooms { get; set; }

    public DbSet<Booking> Bookings { get; set; }
}