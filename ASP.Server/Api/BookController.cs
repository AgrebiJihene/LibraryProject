using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.Server.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.Server.Database;

namespace ASP.Server.Api
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibraryDbContext libraryDbContext;

        public BookController(LibraryDbContext libraryDbContext)
        {
            this.libraryDbContext = libraryDbContext;
        }

        public ActionResult<List<BookDTO>> GetBooks(int? limit, int? offset, int? genre)
        {
            IQueryable<Book> req = libraryDbContext.Books.Include(a => a.Author).Include(b => b.Genres);
            if (genre != null)
            {

                Genre reqInterm = libraryDbContext.Genre.FirstOrDefault(g => g.Id == genre);
                req = req.Where(b => b.Genres.Contains(reqInterm));
            }
            if (offset != null)
            {
                req = req.Skip((int)offset);
            }

            if (limit != null)
            {
                req = req.Take((int)limit);
            }

            return req.Select(x => new BookDTO(x)).ToList();
        }

        public ActionResult<Book> GetBook(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var bookItem = libraryDbContext.Books.Include(a => a.Author).Include(g => g.Genres)
                .FirstOrDefault(b => b.Id == id);

            if (bookItem == null)
            {
                return NotFound();
            }

            return bookItem;
        }

        public async Task<ActionResult<List<Genre>>> GetGenres()
        {
            return await libraryDbContext.Genre
            .Select(x => x)
            .ToListAsync();
        }
    }
}
