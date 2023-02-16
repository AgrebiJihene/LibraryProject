using System.ComponentModel;
using System.Threading.Tasks;
using WPF.Reader.Api;
using WPF.Reader.Model;

namespace WPF.Reader.ViewModel
{
    class ReadBook : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Book CurrentBook { get; private set; }

        public ReadBook(BookDTO book)
        {
            var task = new Task(() =>
            {

                CurrentBook = new BookApi().BookGetBook(book.Id);

            }

            );
            task.Start();
            
        }
    }

    /* Cette classe sert juste a afficher des donnée de test dans le designer */
    class InDesignReadBook : ReadBook
    {
        public InDesignReadBook() : base(new BookDTO())
        {
        }
    }
}
