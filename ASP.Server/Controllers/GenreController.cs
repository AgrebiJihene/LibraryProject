using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.Server.Database;
using ASP.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Server.Controllers
{
    public class GenreController : Controller
    {
        private readonly LibraryDbContext libraryDbContext;

        public GenreController(LibraryDbContext libraryDbContext)
        {
            this.libraryDbContext = libraryDbContext;
        }

        // A vous de faire comme BookController.List mais pour les genres !

        public ActionResult<IEnumerable<Genre>> List()
        {
            // récupérer les genres dans la base de donées pour qu'elle puisse être affiché
            List<Genre> ListGenres = libraryDbContext.Genre.Include(g => g.Books).ToList();
            return View(ListGenres);
        }
    }
}
