using Client.Helpers;
using Common.Contract;
using Common.Model;
using log4net;
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

namespace Client.ViewModels.EventViewModels
{
    public class RemoveEventAlbumViewModel : INotifyPropertyChanged
    {
        private static readonly ILog log = LogHelper.GetLogger();
        private IServices Proxy { get; set; }
        public User User { get; set; }
        public ObservableCollection<Event> MyEvents { get; set; }
        public ObservableCollection<Album> MyAlbums { get; set; }
        public ICommand ShowEventAlbumsCommand { get; set; }
        public ICommand GoBackCommand { get; set; }

        public RemoveEventAlbumViewModel(IServices proxy, User u, ObservableCollection<Event> MyEvents, ObservableCollection<Album> MyAlbums)
        {
            this.Proxy = proxy;
            this.User = u;
            this.MyEvents = MyEvents;
            this.MyAlbums = MyAlbums;
            this.ShowEventAlbumsCommand = new RelayCommand(ShowEventAlbumsExecute, ShowEventAlbumsCanExecute);
            this.GoBackCommand = new RelayCommand(GoBackExecute, GoBackCanExecute);

            log.Debug("RemoveEventAlbum: ViewModel constructor called");
        }

        public bool GoBackCanExecute(object parametar)
        {
            return true;
        }

        public void GoBackExecute(object parametar)
        {
            UserControl uc = parametar as UserControl;

            Window Window = Window.GetWindow(uc);
            log.Info("GoBack: Successfully executed");
            Window.Close();
        }

        public bool ShowEventAlbumsCanExecute(object parametar)
        {
            Event SelectedEvent = parametar as Event;

            if (SelectedEvent == null)
                return false;

            return true;
        }

        public void ShowEventAlbumsExecute(object parametar)
        {
            Event SelectedEvent = parametar as Event;

            Window albumPictures = new Window();
            albumPictures.Height = 550;
            albumPictures.Width = 700;
            albumPictures.Content = new ShowEventAlbumsViewModel(this.Proxy, SelectedEvent);
            albumPictures.Show();
            log.Debug("ShowEventAlbums: Window showed");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
