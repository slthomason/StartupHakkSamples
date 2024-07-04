using DBContexts.Contracts;
using DBContexts.DAL;
using DBContexts.Models;

using Microsoft.AspNetCore.Mvc;

namespace DBContexts.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SampleController : ControllerBase
    {
        protected IRepository<Book, FirstDbContext> BookRepository { get; }

        protected IRepository<Movie, SecondDbContext> MovieRepository { get; }

        public SampleController(IRepository<Book, FirstDbContext> bookRepository, IRepository<Movie, SecondDbContext> movieRepository)
        {
            BookRepository = bookRepository;
            MovieRepository = movieRepository;
        }

        [HttpGet(Name = "get-books")]
        public async Task<IEnumerable<Book>> GetBooks()
        {
            var book = new Book
            {
                Name = "Book " + Guid.NewGuid()
            };

            BookRepository.Add(book);

            await BookRepository.SaveChangesAsync();

            return BookRepository.GetAll();
        }

        [HttpGet(Name = "get-movies")]
        public async Task<IEnumerable<Movie>> GetMovies()
        {
            var movie = new Movie
            {
                Name = "Movie " + Guid.NewGuid()
            };

            MovieRepository.Add(movie);

            await MovieRepository.SaveChangesAsync();

            return MovieRepository.GetAll();
        }
    }
}