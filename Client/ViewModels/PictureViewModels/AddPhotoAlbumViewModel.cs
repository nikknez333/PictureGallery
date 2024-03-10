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

namespace Client.ViewModels.PictureViewModels
{
    public class AddPhotoAlbumViewModel : INotifyPropertyChanged
    {
        private static readonly ILog log = LogHelper.GetLogger();
        private IServices Proxy { get; set; }
        public User User { get; set; }
        public ObservableCollection<Picture>Pictures { get; set; }
        public ObservableCollection<Album> MyAlbums { get; set; }

        public ICommand AddPhotoAlbumCommand { get; set; }
        public ICommand GoBackCommand { get; set; }

        public AddPhotoAlbumViewModel(IServices proxy, User u, ObservableCollection<Picture> Pictures, ObservableCollection<Album> Albums)
        {
            this.Proxy = proxy;
            this.User = u;
            this.Pictures = Pictures;
            this.MyAlbums = Albums;
            this.AddPhotoAlbumCommand = new RelayCommand(AddPhotoAlbumExecute, AddPhotoAlbumCanExecute);
            this.GoBackCommand = new RelayCommand(GoBackExecute, GoBackCanExecute);
            log.Debug("AddPhotoAlbum: ViewModel constructor called");
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

        public bool AddPhotoAlbumCanExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            Album SelectedAlbum = parametars[0] as Album;
            Picture SelectedPicture = parametars[1] as Picture;

            if (SelectedAlbum == null || SelectedPicture == null)
                return false;

            return true;
        }

        public void AddPhotoAlbumExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            Album SelectedAlbum = parametars[0] as Album;
            Picture SelectedPicture = parametars[1] as Picture;

            bool IsSuccessfullyExecuted = false;

            try
            {
                IsSuccessfullyExecuted = Proxy.AddPhotoToAlbum(SelectedAlbum, SelectedPicture);
            }
            catch(Exception e)
            {
                log.Error(String.Format("AddPhotoAlbum Server Error: {0}", e.Message));
            }

            if (IsSuccessfullyExecuted)
                log.Info("AddPhotoAlbum: Successfully executed");
            else
                log.Error("AddPhotoAlbum: Failed to execute");

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
