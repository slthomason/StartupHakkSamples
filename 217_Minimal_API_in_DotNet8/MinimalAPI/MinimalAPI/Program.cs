using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IBookService, BookService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Hello Word
app.MapGet("/", () => "Hello, World!");

// Example 1: Get all books
app.MapGet("/books", (IBookService bookService) =>
    TypedResults.Ok(bookService.GetBooks()))
    .WithName("GetBooks");

// Example 2: Get a specific book by ID
app.MapGet("/books/{id}", Results<Ok<Book>, NotFound> (IBookService bookService, int id) =>
{
    var book = bookService.GetBook(id);
    return book is { } ? TypedResults.Ok(book) : TypedResults.NotFound();
}).WithName("GetBookById");

// Example 3: Add a new book
app.MapPost("/books", (IBookService bookService, Book newBook) =>
{
    bookService.AddBook(newBook);
    return TypedResults.Created($"/books/{newBook.Id}", newBook);
}).WithName("AddBook");

// Example 4: Update an existing book
app.MapPut("/books/{id}", (IBookService bookService, int id, Book updatedBook) =>
{
    bookService.UpdateBook(id, updatedBook);
    return TypedResults.Ok();
}).WithName("UpdateBook");

// Example 5: Delete a book by ID
app.MapDelete("/books/{id}", (IBookService bookService, int id) =>
{
    bookService.DeleteBook(id);
    return TypedResults.NoContent();
}).WithName("DeleteBook");

app.Run();



public interface IBookService
{
    List<Book> GetBooks();

    Book GetBook(int id);

    void AddBook(Book book);

    void UpdateBook(int id, Book updatedBook);

    void DeleteBook(int id);
}

public class BookService : IBookService
{
    private readonly List<Book> _books;

    public BookService()
    {
        _books = new List<Book>
            {
               new Book
               {
                   Id = 1,
                   Title = "Clean Code: A Handbook of Agile Software Craftsmanship",
                   Author = "Robert C. Martin"
               },
                new Book
                {
                    Id = 2,
                    Title = "Design Patterns: Elements of Reusable Object-Oriented Software",
                    Author = "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides"
                },
                new Book
                {
                    Id = 3,
                    Title = "Refactoring: Improving the Design of Existing Code",
                    Author = "Martin Fowler"
                },
                new Book
                {
                    Id = 4,
                    Title = "Code Complete: A Practical Handbook of Software Construction",
                    Author = "Steve McConnell"
                }
            };
    }

    public List<Book> GetBooks()
    {
        return _books;
    }

    public Book GetBook(int id)
    {
        return _books.FirstOrDefault(x => x.Id == id);
    }

    public void AddBook(Book book)
    {
        _books.Add(book);
    }

    public void UpdateBook(int id, Book updatedBook)
    {
        var existingBook = _books.FirstOrDefault(x => x.Id == id);
        if (existingBook != null)
        {
            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
        }
    }

    public void DeleteBook(int id)
    {
        var bookToRemove = _books.FirstOrDefault(x => x.Id == id);
        if (bookToRemove != null)
        {
            _books.Remove(bookToRemove);
        }
    }
}

public class Book
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Author { get; set; }
}