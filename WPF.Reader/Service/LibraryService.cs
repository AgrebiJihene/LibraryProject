using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using WPF.Reader.Api;
using WPF.Reader.Model;

namespace WPF.Reader.Service
{
    public class LibraryService
    {

        public ObservableCollection<BookDTO> Books { get; set; } = new ObservableCollection<BookDTO>();
        public ObservableCollection<Genre> Genres { get; set; } = new ObservableCollection<Genre>();


        public LibraryService()
        {
        }

        public async void UpdateBooks(Genre genre = null)
        { 

            var books = await new BookApi().BookGetBooksAsync(genre:genre?.Id);
            //Thread.Sleep(10000);
            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    Books.Clear();
                    foreach (var book in books)
                    {
                        Books.Add(book);
                    }
                }
                );
        }

        public async void UpdateGenres()
        {
            var genres = await new BookApi().BookGetGenresAsync();
            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    Genres.Clear();
                    foreach (var genre in genres)
                    {
                        Genres.Add(genre);
                    }
                }
                );
        }
    }
}
