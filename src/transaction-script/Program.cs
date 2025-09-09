// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using transaction_script;
using transaction_script.Models;
using transaction_script.Services;

var options = new DbContextOptionsBuilder<HotelContext>()
    .UseSqlite("Data Source=hotel.db")
    .Options;

using (var initContext = new HotelContext(options))
{
    await initContext.Database.EnsureDeletedAsync();
    await initContext.Database.EnsureCreatedAsync();

    await initContext.Rooms.AddAsync(new Room { Id = 1, Number = "101", Available = true });
    await initContext.Rooms.AddAsync(new Room { Id = 2, Number = "102", Available = true });
    await initContext.SaveChangesAsync();
}

using (var db = new HotelContext(options))
{
    var bookingService = new BookingService(db);
    Console.WriteLine("Booking room 101 for Numan...");
    var bookingId = await bookingService.BookRoomAsync(
        1,
        "Numan",
        DateTime.Today,
        DateTime.Today.AddDays(3)
    );
    Console.WriteLine($"Booking successful! BookingId: {bookingId}");

    // Console.WriteLine($"Cancelling booking {bookingId}...");
    // await bookingService.CancelBookingAsync(bookingId);
    // Console.WriteLine("Booking cancelled!");
}