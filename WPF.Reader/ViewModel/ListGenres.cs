using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF.Reader.Model;
using WPF.Reader.Service;


namespace WPF.Reader.ViewModel
{
    internal class ListGenres : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ItemSelectedCommand { get; set; }

        public ObservableCollection<Genre> Genres => Ioc.Default.GetRequiredService<LibraryService>().Genres;
        public ListGenres()
        {

            var task = new Task(() =>
            {
                Ioc.Default.GetRequiredService<LibraryService>().UpdateGenres();
            }
            );
            task.Start();
            ItemSelectedCommand = new RelayCommand(e =>
            {
                if (((SelectionChangedEventArgs)e).AddedItems.Count == 0)
                    return;
                Genre genre = ((SelectionChangedEventArgs)e).AddedItems[0] as Genre;
                Ioc.Default.GetRequiredService<INavigationService>().Navigate<ListBook>(genre);
            });
        }

    }
}
