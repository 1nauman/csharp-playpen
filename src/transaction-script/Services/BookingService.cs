using transaction_script.Models;

namespace transaction_script.Services;

public class BookingService(HotelContext context)
{
    public async Task<int> BookRoomAsync(int roomId, string guestName, DateTime checkIn, DateTime checkOut)
    {
        if (checkOut <= checkIn)
        {
            throw new InvalidOperationException("CheckOut must be after checkIn.");
        }

        var room = await context.Rooms.FindAsync(roomId);
        if (room == null)
        {
            throw new InvalidOperationException("Room not found.");
        }

        if (!room.Available)
        {
            throw new InvalidOperationException("Room not available.");
        }

        room.Available = false;

        var booking = new Booking
        {
            RoomId = room.Id,
            GuestName = guestName,
            CheckOut = checkOut,
            CheckIn = checkIn,
            Price = CalculatePrice(checkIn, checkOut)
        };

        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        return booking.Id;
    }

    public async Task CancelBookingAsync(int bookingId)
    {
        var booking = await context.Bookings.FindAsync(bookingId);

        if (booking == null)
        {
            throw new InvalidOperationException("Booking not found.");
        }

        var room = await context.Rooms.FindAsync(booking.RoomId);

        if (room != null)
        {
            room.Available = true;
        }

        context.Bookings.Remove(booking);
        await context.SaveChangesAsync();
    }

    private static decimal CalculatePrice(DateTime checkIn, DateTime checkOut)
    {
        var nights = (checkOut - checkIn).Days;
        return nights * 10000;
    }
}