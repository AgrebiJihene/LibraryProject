using CommunityToolkit.Mvvm.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF.Reader.Model;
using WPF.Reader.Service;

namespace WPF.Reader.ViewModel
{
    internal class ListBook : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ItemSelectedCommand { get; set; }

        // n'oublier pas faire de faire le binding dans ListBook.xaml !!!!
        public ObservableCollection<Book> Books => Ioc.Default.GetRequiredService<LibraryService>().Books;

        public ListBook()
        {
            ItemSelectedCommand = new RelayCommand(e => {
                /* the livre devrais etre dans la variable book */
                Book book = ((SelectionChangedEventArgs)e).AddedItems[0] as Book;
                //MessageBox.Show(book.Title);
                
                Ioc.Default.GetRequiredService<INavigationService>().Navigate<DetailsBook>(book);
            });
        }
    }
}
