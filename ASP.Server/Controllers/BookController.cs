using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.Server.Database;
using ASP.Server.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ASP.Server.Controllers
{
    public class CreateBookModel
    {
        [Required]
        [Display(Name = "Titre")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Author")]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Contenu")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Prix")]
        public double? Price { get; set; } = null;

        public List<int> Genres { get; set; } = new();

        public IEnumerable<Genre> AllGenres { get; init; }
    }

    public class EditBookModel
    {
        public int Id { get; set; }

        [Display(Name = "Titre")]
        public string Title { get; set; }

        [Display(Name = "Author")]
        public string Author { get; set; }

        [Display(Name = "Contenu")]
        public string Content { get; set; }

        [Display(Name = "Prix")]
        public double? Price { get; set; } = null;

        public List<int> Genres { get; set; } = new();

        public IEnumerable<Genre> AllGenres { get; init; }
    }

    public class BookController : Controller
    {
        private readonly LibraryDbContext libraryDbContext;

        public BookController(LibraryDbContext libraryDbContext)
        {
            this.libraryDbContext = libraryDbContext;
        }

        public ActionResult<IEnumerable<Book>> List()
        {
            List<Book> ListBooks = libraryDbContext.Books.Include(a => a.Author).Include(b => b.Genres).ToList();
            return View(ListBooks);
        }

        public ActionResult<CreateBookModel> Create(CreateBookModel book)
        {
            List<Genre> genres = libraryDbContext.Genre.ToList();

            if (ModelState.IsValid)
            {
                Author newAuthor = libraryDbContext.Author.Where(n => n.Name == book.Author).SingleOrDefault();

                if (newAuthor == null) {
                    newAuthor = new Author() { Name = book.Author };
                }

                List<Genre> newGenres = new List<Genre>();

                foreach (int id in book.Genres)
                {
                    newGenres.Add(libraryDbContext.Genre.Find(id));
                }

                if (book.Price < 0)
                {
                    return RedirectToAction(nameof(Create));
                }

                libraryDbContext.Add(new Book() { Title = book.Title, Author = newAuthor, Content = book.Content, Price = (double)book.Price, Genres = newGenres });
                libraryDbContext.SaveChanges();

                return RedirectToAction(nameof(List));
            }

            return View(new CreateBookModel() { AllGenres = genres });
        }
        public ActionResult Delete(int id)
        {
            libraryDbContext.Books.Remove(libraryDbContext.Books.Find(id));
            libraryDbContext.SaveChanges();

            return RedirectToAction(nameof(List));
        }

        public ActionResult ViewEditPage(int id)
        {
            return RedirectToAction(nameof(GetBookToEdit), new { id });
        }

        public ActionResult<EditBookModel> GetBookToEdit(int id)
        {
            Book book = libraryDbContext.Books.Include(a => a.Author).Include(b => b.Genres).Where(book => book.Id == id).Single();
            List<int> selectedGenres = new List<int>();
            List<Genre> genres = libraryDbContext.Genre.ToList();

            foreach (var genre in book.Genres)
            {
                selectedGenres.Add(genre.Id);
            }

            return View("Edit", new EditBookModel() { Id = book.Id, Title = book.Title, Author = book.Author.Name, Content = book.Content, Price = book.Price, Genres = selectedGenres, AllGenres = genres });
        }

        public ActionResult EditBook(EditBookModel bookForm)
        {
            Book book = libraryDbContext.Books.Include(b => b.Genres).Where(book => book.Id == bookForm.Id).Single();
            List<Genre> genres = new();

            if (bookForm.Title == null || bookForm.Author == null || bookForm.Content == null || bookForm.Price == null || bookForm.Price < 0)
            {
                return RedirectToAction(nameof(GetBookToEdit), new { bookForm.Id });
            }

            Author newAuthor = libraryDbContext.Author.Where(n => n.Name == bookForm.Author).SingleOrDefault();

            if (newAuthor == null)
            {
                newAuthor = new Author() { Name = bookForm.Author };
            }

            book.Title = bookForm.Title;
            book.Author = newAuthor;
            book.Content = bookForm.Content;
            book.Price = (double)bookForm.Price;

            foreach (var idGenre in bookForm.Genres)
            {
                Genre genre = libraryDbContext.Genre.Find(idGenre);
                genres.Add(genre);
            }

            book.Genres = genres;

            libraryDbContext.Update(book);

            int nb = libraryDbContext.SaveChanges();

            if (nb > 0)
            {
                return RedirectToAction(nameof(List));
            }
            else
            {
                return RedirectToAction(nameof(GetBookToEdit), new { bookForm.Id });
            }
        }
    }
}
