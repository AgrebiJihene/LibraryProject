using ASP.Server.Model;
using System.Linq;

namespace ASP.Server.Database
{
    public class DbInitializer
    {
        public static void Initialize(LibraryDbContext bookDbContext)
        {
            if (bookDbContext.Books.Any())
                return;

            Author a1, a2, a3, a4;
            Genre SF, Classic, Romance, Thriller;

            bookDbContext.Author.AddRange(
                a1 = new Author() { Name = "a1" },
                a2 = new Author() { Name = "a2" },
                a3 = new Author() { Name = "a3" },
                a4 = new Author() { Name = "a4" }
            );

            bookDbContext.Genre.AddRange(
                SF = new Genre() { Name = "SF" },
                Classic = new Genre() { Name = "Classic" },
                Romance = new Genre() { Name = "Romance" },
                Thriller = new Genre() { Name = "Thriller" }
            );
            bookDbContext.SaveChanges();

            bookDbContext.Books.AddRange(
                new Book() { Title = "book1", Author = a1, Content = "content1", Price = 10.6, Genres = new() { Romance, Thriller } },
                new Book() { Title = "book2", Author = a2, Content = "content2", Price = 28.6, Genres = new() { SF, Thriller } },
                new Book() { Title = "book3", Author = a3, Content = "content3", Price = 74.6, Genres = new() { Classic, Thriller } },
                new Book() { Title = "book4", Author = a4, Content = "content4", Price = 18.7, Genres = new() { Romance, Classic } }
            );
            bookDbContext.SaveChanges();
        }
    }
}