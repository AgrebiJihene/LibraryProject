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
        public ObservableCollection<Genre> Genres { get; set; }

        // A remplacer avec vos propre données !!!!!!!!!!!!!! ContentPresenter MessageBox
        // Pensé qu'il ne faut mieux ne pas réaffecter la variable Books, mais juste lui ajouter et / ou enlever des éléments
        // Donc pas de LibraryService.Instance.Books = ...
        // mais plutot LibraryService.Instance.Books.Add(...)
        // ou LibraryService.Instance.Books.Clear()
        public ObservableCollection<BookDTO> Books { get; set; } = new ObservableCollection<BookDTO>();

        public LibraryService()
        {
            UpdateBooks();
        }

        public async void UpdateBooks()
        {
            var books = await new BookApi().BookGetBooksAsync();
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
    }
}
