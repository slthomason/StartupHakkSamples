public class Query
{
    private static List<Book> books = new List<Book>
    {
        new Book { Title = "GraphQL for beginners", Author = "John Doe" },
        new Book { Title = "Mastering GraphQL", Author = "Jane Doe" }
    };

    public List<Book> GetBooks() => books;
}