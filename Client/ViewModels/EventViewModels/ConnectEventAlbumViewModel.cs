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
    public class ConnectEventAlbumViewModel : INotifyPropertyChanged
    {
        private static readonly ILog log = LogHelper.GetLogger();
        private IServices Proxy { get; set; }
        public User User { get; set; }
        public ObservableCollection<Event> MyEvents { get; set; }
        public ObservableCollection<Album> MyAlbums { get; set; }
        public ICommand ConnectAlbumEventCommand { get; set; }
        public ICommand GoBackCommand { get; set; }

        public ConnectEventAlbumViewModel(IServices proxy, User u, ObservableCollection<Event> MyEvents, ObservableCollection<Album> MyAlbums)
        {
            this.Proxy = proxy;
            this.User = u;
            this.MyEvents = MyEvents;
            this.MyAlbums = MyAlbums;
            this.ConnectAlbumEventCommand = new RelayCommand(ConnectAlbumEventExecute, ConnectAlbumEventCanExecute);
            this.GoBackCommand = new RelayCommand(GoBackExecute, GoBackCanExecute);

            log.Debug("ConnectEventAlbum: ViewModel constructor called");
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

        public bool ConnectAlbumEventCanExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            Album SelectedAlbum = parametars[0] as Album;
            Event SelectedEvent = parametars[1] as Event;

            if (SelectedAlbum == null || SelectedEvent == null)
                return false;

            return true;
        }

        public void ConnectAlbumEventExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            Album SelectedAlbum = parametars[0] as Album;
            Event SelectedEvent = parametars[1] as Event;

            bool IsSuccessfullyExecuted = false;

            try
            {
                IsSuccessfullyExecuted = Proxy.ConnectAlbumToEvent(SelectedAlbum, SelectedEvent);
            }
            catch(Exception e)
            {
                log.Error(String.Format("ConnectAlbumToEvent Server Error: {0}", e.Message));
            }

            if (IsSuccessfullyExecuted)
                log.Info("ConnectAlbumEvent: Successfully executed");
            else
                log.Error("ConnectAlbumEvent: Failed to execute");

            UserControl uc = parametars[2] as UserControl;

            Window window = Window.GetWindow(uc);
            window.Close();
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
