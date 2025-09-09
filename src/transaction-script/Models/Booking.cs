namespace transaction_script.Models;

public class Booking
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public string GuestName { get; set; } = string.Empty;

    public DateTime CheckIn { get; set; }

    public DateTime CheckOut { get; set; }

    public decimal Price { get; set; }
}