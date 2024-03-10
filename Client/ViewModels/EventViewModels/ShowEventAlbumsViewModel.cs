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
    public class ShowEventAlbumsViewModel : INotifyPropertyChanged
    {
        private static readonly ILog log = LogHelper.GetLogger();
        private IServices Proxy { get; set; }
        public ICommand RemoveAlbumEventCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public Event SelectedEvent { get; set; }
        public ObservableCollection<Album> MyEventAlbums { get; set; }

        public ShowEventAlbumsViewModel(IServices proxy, Event SelectedEvent)
        {
            this.Proxy = proxy;
            this.SelectedEvent = SelectedEvent;
            this.GoBackCommand = new RelayCommand(GoBackExecute, GoBackCanExecute);
            this.RemoveAlbumEventCommand = new RelayCommand(RemoveAlbumEventExecute, RemoveAlbumEventCanExecute);
            this.MyEventAlbums = new ObservableCollection<Album>(Proxy.GetEventAlbums(this.SelectedEvent));

            log.Debug("ShowEventAlbums: ViewModel constructor called");
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

        public bool RemoveAlbumEventCanExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            Album SelectedAlbum = parametars[0] as Album;

            if (SelectedAlbum == null)
                return false;

            return true;
        }

        public void RemoveAlbumEventExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            Album SelectedAlbum = parametars[0] as Album;

            bool IsSuccessfullyExecuted = false;

            try
            {
                IsSuccessfullyExecuted = Proxy.RemoveAlbumFromEvent(SelectedAlbum, SelectedEvent);
            }
            catch(Exception e)
            {
                log.Error(String.Format("RemoveAlbumEvent Server Error: {0}", e.Message));
            }

            if (IsSuccessfullyExecuted)
                log.Info("RemoveAlbumEvent: Successfully executed");
            else
                log.Error("RemoveAlbumEvent: Failed to execute");

            UserControl uc = parametars[1] as UserControl;

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
