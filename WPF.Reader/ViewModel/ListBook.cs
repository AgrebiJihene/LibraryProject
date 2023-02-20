using CommunityToolkit.Mvvm.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
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
        public ICommand IncreaseValueCommand { get; set; }
        public ICommand DecreaseValueCommand { get; set; }
        public ICommand InitilisationList { get; set; }


        public int Offset { get; set; } = 0;


        // n'oublier pas faire de faire le binding dans ListBook.xaml !!!!
        public ObservableCollection<BookDTO> Books => Ioc.Default.GetRequiredService<LibraryService>().Books;

        public ListBook() : this(null)
        {
        }
        public ListBook(Genre genre = null)
        {
            var task = new Task(() =>
            {
                Ioc.Default.GetRequiredService<LibraryService>().UpdateBooks(Offset,genre);
            }

           );
            task.Start();
            ItemSelectedCommand = new RelayCommand(e => {
                if (((SelectionChangedEventArgs)e).AddedItems.Count == 0)
                    return;
                /* the livre devrais etre dans la variable book */
                BookDTO book = ((SelectionChangedEventArgs)e).AddedItems[0] as BookDTO;
                
                Ioc.Default.GetRequiredService<INavigationService>().Navigate<DetailsBook>(book);
            });


            IncreaseValueCommand = new RelayCommand(o => {
                IncreaseValue();
                var task = new Task(() =>
                {
                    Ioc.Default.GetRequiredService<LibraryService>().UpdateBooks(Offset, genre);
                }

                );
                task.Start();
            }, o => true);

            DecreaseValueCommand = new RelayCommand(o => {
                DecreaseValue();
                var task = new Task(() =>
                {
                    Ioc.Default.GetRequiredService<LibraryService>().UpdateBooks(Offset, genre);
                }

                );
                task.Start();
            }, o => true);

            InitilisationList = new RelayCommand(o => {
                var task = new Task(() =>
                {
                    Ioc.Default.GetRequiredService<LibraryService>().UpdateBooks(0, genre);
                }

                );
                task.Start();
            }, o => true);

        }
        public void IncreaseValue()
        {
            Offset += 5;
        }

        public void DecreaseValue()
        {
            Offset -= 5;
        }
    }

}
