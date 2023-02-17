using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Reader.Service;

namespace WPF.Reader.ViewModel
{
    internal class Acceuil: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand PageBooksSelectedCommand { get; init; }
        public ICommand PageGenresSelectedCommand { get; init; }

        public Acceuil()
        {
            PageBooksSelectedCommand = new RelayCommand(x => {
                Ioc.Default.GetRequiredService<INavigationService>().Navigate<ListBook>();
            });

            PageGenresSelectedCommand = new RelayCommand(x => {
                Ioc.Default.GetRequiredService<INavigationService>().Navigate<ListGenres>();
            });
        }

    }
}
