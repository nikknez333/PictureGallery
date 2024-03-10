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

namespace Client.ViewModels.AlbumViewModels
{
    public class ShowAlbumViewModel : INotifyPropertyChanged
    {
        private static readonly ILog log = LogHelper.GetLogger();
        public User User { get; set; }
        private IServices Proxy { get; set; }
        public ObservableCollection<Album> AllAlbums { get; set; }

        public ICommand GoBackCommand { get; set; }
        public ICommand ModifyAlbumCommand { get; set; }
        public ICommand DeleteAlbumCommand { get; set; }

        public ShowAlbumViewModel(User u, IServices proxy, ObservableCollection<Album> Albums)
        {
            this.GoBackCommand = new RelayCommand(GoBackExecute, GoBackCanExecute);
            this.ModifyAlbumCommand = new RelayCommand(ModifyAlbumExecute, ModifyAlbumCanExecute);
            this.DeleteAlbumCommand = new RelayCommand(DeleteAlbumExecute, DeleteAlbumCanExecute);
            this.Proxy = proxy;
            this.User = u;
            this.AllAlbums = Albums;

            log.Debug("ShowAlbum: ViewModel constructor called");
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

        public bool ModifyAlbumCanExecute(object parametar)
        {
            Album SelectedAlbum = parametar as Album;

            if (parametar == null || SelectedAlbum == null)
                return false;
            return true;
        }

        public void ModifyAlbumExecute(object parametar)
        {
            Album SelectedAlbum = parametar as Album;

            Window modifyAlbum = new Window();
            modifyAlbum.Height = 550;
            modifyAlbum.Width = 450;
            modifyAlbum.Content = new ModifyAlbumViewModel(SelectedAlbum, Proxy);
            modifyAlbum.ShowDialog();
            log.Debug("ModifyAlbum: Window showed");
        }

        public bool DeleteAlbumCanExecute(object parametar)
        {
            Album SelectedAlbum = parametar as Album;

            if (parametar == null || SelectedAlbum == null)
                return false;
            return true;
        }

        public void DeleteAlbumExecute(object parametar)
        {
            Album SelectedAlbum = parametar as Album;

            bool IsSuccessfullyExecuted = false;

            try
            {
                IsSuccessfullyExecuted = Proxy.DeleteAlbum(SelectedAlbum);
            }
            catch(Exception e)
            {
                log.Error(String.Format("DeleteAlbum Server Error: {0}", e.Message));
            }

            if (IsSuccessfullyExecuted)
                log.Info("DeleteAlbum: Successfully executed");
            else
                log.Error("DeleteAlbum: Failed to execute");

            AllAlbums.Remove(SelectedAlbum);
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
