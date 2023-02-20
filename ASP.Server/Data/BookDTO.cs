using ASP.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Server.Model
{
    public class BookDTO
    {
        private Book b;
        public BookDTO(Book b) {
            this.b = b;
        }
        public int Id { get => b.Id; }
        public string Title { get => b.Title; }
        public double Price { get => b.Price; }
        public Author Author { get => b.Author; }

        public List<Genre> Genres { get => b.Genres; }

    }
}
