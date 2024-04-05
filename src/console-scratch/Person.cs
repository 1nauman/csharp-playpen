namespace console_scratch;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
}

public record StockPrice(DateOnly Date, decimal Price);