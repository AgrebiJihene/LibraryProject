using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ASP.Server.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ASP.Server.Database;
using ASP.Server.Model;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ASP.Server.Controllers
{
    public class StatistiqueModel
    {
        [Required]
        [Display(Name = "NbBooks")]
        public int NbBooks { get; set; }

        [Required]
        [Display(Name = "MinWords")]
        public int MinWords { get; set; }

        [Required]
        [Display(Name = "MaxWords")]
        public int MaxWords { get; set; }

        [Required]
        [Display(Name = "MedWords")]
        public double MedWords { get; set; }

        [Required]
        [Display(Name = "AvgWords")]
        public double AvgWords { get; set; }

        [Required]
        [Display(Name = "NbBooksByAuthor")]
        public Dictionary<string, int> NbBooksByAuthor { get; set; }
    }

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LibraryDbContext libraryDbContext;

        public HomeController(ILogger<HomeController> logger, LibraryDbContext libraryDbContext)
        {
            _logger = logger;
            this.libraryDbContext = libraryDbContext;
        }

        public IActionResult Index()
        {
            List<Author> authors = libraryDbContext.Author.Include(a => a.Books).ToList();
            List<Book> books = libraryDbContext.Books.ToList();
            List<string> contents = new();
            List<int> contentsLenght = new();
            int nbBooks = libraryDbContext.Books.Count();
            int minWords = -1;
            int maxWords = 0;
            double medWords = 0.0;
            double avgWords = 0.0;
            Dictionary<string, int> nbBooksByAuthor = new();

            foreach (Book book in books)
            {
                string content = book.Content;

                if (content != null)
                {
                    contents.Add(content);
                }
            }

            contentsLenght = contents.Select(c => c.Length).ToList();

            contentsLenght.Sort();

            minWords = contentsLenght.Min();
            maxWords = contentsLenght.Max();

            int middle = contentsLenght.Count() / 2;

            if (contentsLenght.Count() % 2 == 0)
            {
                medWords = (contentsLenght[middle - 1] + contentsLenght[middle]) / 2;
            }
            else
            {
                medWords = contentsLenght[middle / 2];
            }

            avgWords = contentsLenght.Average();

            foreach (Author author in authors)
            {
                string authorName = author.Name;
                int authorNbBook = author.Books.Count();

                nbBooksByAuthor[authorName] = authorNbBook;
            }

            return View(new StatistiqueModel() { NbBooks = nbBooks, MinWords = minWords, MaxWords = maxWords, MedWords = medWords, AvgWords = avgWords, NbBooksByAuthor = nbBooksByAuthor });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
