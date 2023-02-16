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

        // Ajouter ici tous les champ que l'utilisateur devra remplir pour ajouter un livre
        [Required]
        [Display(Name = "Contenu")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Prix")]
        public double? Price { get; set; } = null;
        // Liste des genres séléctionné par l'utilisateur
        public List<int> Genres { get; set; } = new();

        // Liste des genres a afficher à l'utilisateur
        public IEnumerable<Genre> AllGenres { get; init; }
    }

    public class EditBookModel
    {
        public int Id { get; set; }
        [Display(Name = "Titre")]
        public string Title { get; set; }

        [Display(Name = "Contenu")]
        public string Content { get; set; }

        [Display(Name = "Prix")]
        public double? Price { get; set; } = null;
        // Liste des genres séléctionné par l'utilisateur
        public List<int> Genres { get; set; } = new();

        // Liste des genres a afficher à l'utilisateur
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
            // récupérer les livres dans la base de donées pour qu'elle puisse être affiché
            List<Book> ListBooks = libraryDbContext.Books.Include(b => b.Genres).ToList();
            return View(ListBooks);
        }

        public ActionResult<CreateBookModel> Create(CreateBookModel book)
        {
            // Il faut intéroger la base pour récupérer l'ensemble des objets genre qui correspond aux id dans CreateBookModel.Genres
            List<Genre> genres = libraryDbContext.Genre.ToList();

            // Le IsValid est True uniquement si tous les champs de CreateBookModel marqués Required sont remplis
            if (ModelState.IsValid)
            {
                // Completer la création du livre avec toute les information nécéssaire que vous aurez ajoutez, et metter la liste des gener récupéré de la base aussi
                List<Genre> newGenres = new List<Genre>();

                foreach (int id in book.Genres)
                {
                    newGenres.Add(libraryDbContext.Genre.Find(id));
                }

                if (book.Price < 0)
                {
                    return RedirectToAction(nameof(Create));
                }

                libraryDbContext.Add(new Book() { Title = book.Title, Content = book.Content, Price = (double)book.Price, Genres = newGenres });
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

            List<Genre> genres = libraryDbContext.Genre.ToList();
            Book book = libraryDbContext.Books.Include(b => b.Genres).Where(book => book.Id == id).Single();
            List<int> selectedGenres = new List<int>();


            foreach (var genre in book.Genres)
            {
                selectedGenres.Add(genre.Id);
            }

            return View("Edit", new EditBookModel() { Id = book.Id, Title = book.Title, Content = book.Content, Price = book.Price, Genres = selectedGenres, AllGenres = genres });

        }

        public ActionResult EditBook(EditBookModel bookForm)
        {
            Book book = libraryDbContext.Books.Include(b => b.Genres).Where(book => book.Id == bookForm.Id).Single();

            if (bookForm.Title == null || bookForm.Content == null || bookForm.Price == null || bookForm.Price < 0)
            {
                return RedirectToAction(nameof(GetBookToEdit), new { bookForm.Id });
            }

            book.Title = bookForm.Title;
            book.Content = bookForm.Content;
            book.Price = (double)bookForm.Price;
            List<Genre> genres = new();
            foreach (var idGenre in bookForm.Genres)
            {
                Genre genre = libraryDbContext.Genre.Find(idGenre);
                genres.Add(genre);
            }
            book.Genres = genres;
            //TryUpdateModelAsync(book);
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
