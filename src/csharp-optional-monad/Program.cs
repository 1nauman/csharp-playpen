// See https://aka.ms/new-console-template for more information

var p1 = Person.Create("Mark", "Twain");
var p2 = Person.Create("Socrates");
Person? p3 = null;

var tomSwayer = Book.Create("The Adventures of Tom Sawyer", p1);
var republic = Book.Create("The Republic", p2);
var bible = Book.Create("Bible");

Console.WriteLine(GetBookLabel(tomSwayer));
Console.WriteLine(GetBookLabel(republic));
Console.WriteLine(GetBookLabel(bible));


string GetBookLabel(Book book) =>
    book
        .Author
        .Map(GetLabel)
        .Map(author => $"{book.Title} by {author}")
        .Reduce(book.Title);

string GetLabel(Person person) =>
    person.LastName.Map(ln => $"{person.FirstName} {ln}").Reduce(person.FirstName);

class Optional<T> where T : class
{
    private T? _value = null;

    public static Optional<T> Some(T obj) => new() { _value = obj };
    public static Optional<T> None() => new();

    public Optional<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class =>
        _value is null ? Optional<TResult>.None() : Optional<TResult>.Some(map(_value));

    public T Reduce(T defaultValue) => _value ?? defaultValue;
}

class Person
{
    public string FirstName { get; set; }
    public Optional<string> LastName { get; set; }

    private Person(string firstName, Optional<string> lastName) => (FirstName, LastName) = (firstName, lastName);

    public static Person Create(string firstName, string lastName) => new(firstName, Optional<string>.Some(lastName));

    public static Person Create(string name) => new(name, Optional<string>.None());
}

class Book
{
    public string Title { get; set; }
    public Optional<Person> Author { get; set; }

    private Book(string title, Optional<Person> author) => (Title, Author) = (title, author);

    public static Book Create(string title, Person author) => new(title, Optional<Person>.Some(author));

    public static Book Create(string title) => new(title, Optional<Person>.None());
}