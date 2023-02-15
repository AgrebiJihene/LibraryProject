using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.Server.Database;
using ASP.Server.Model;
using System;
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
        public float Price { get; set; }
        // Liste des genres séléctionné par l'utilisateur
        public List<int> Genres { get; set; } = new();

        // Liste des genres a afficher à l'utilisateur
        public IEnumerable<Genre> AllGenres { get; init;  }
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
                
                libraryDbContext.Add(new Book() { Title= book.Title, Content= book.Content, Price=book.Price, Genres=newGenres });
                libraryDbContext.SaveChanges();

                return RedirectToAction(nameof(List));
            }
          

            // Il faut interoger la base pour récupérer tous les genres, pour que l'utilisateur puisse les slécétionné
            return View(new CreateBookModel() { AllGenres = genres });
        }
        public ActionResult Delete(int id)
        {
            libraryDbContext.Books.Remove(libraryDbContext.Books.Find(id));
            libraryDbContext.SaveChanges();

            return RedirectToAction(nameof(List));
        }
    }
}
