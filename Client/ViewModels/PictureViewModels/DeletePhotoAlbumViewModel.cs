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
    public class DeletePhotoAlbumViewModel : INotifyPropertyChanged
    {
        private static readonly ILog log = LogHelper.GetLogger();
        private IServices Proxy { get; set; }

        public User User { get; set; }
        public ObservableCollection<Album> MyAlbums { get; set; }

        public ICommand DeletePhotoAlbumCommand { get; set; }
        public ICommand ShowAlbumPicturesCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
       
        public DeletePhotoAlbumViewModel(IServices proxy, User u, ObservableCollection<Album> MyAlbums)
        {
            this.Proxy = proxy;
            this.User = u;
            this.MyAlbums = MyAlbums;
            this.ShowAlbumPicturesCommand = new RelayCommand(ShowAlbumPicturesExecute, ShowAlbumPicturesCanExecute);
            this.GoBackCommand = new RelayCommand(GoBackExecute, GoBackCanExecute);
            log.Debug("DeletePhotoAlbum: ViewModel constructor called");
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

        public bool ShowAlbumPicturesCanExecute(object parametar)
        {
            Album SelectedAlbum = parametar as Album;

            if (SelectedAlbum == null || (SelectedAlbum.IsPrivate = true && SelectedAlbum.CreatedByUser != User.Username))
                return false;
                
            return true;
        }

        public void ShowAlbumPicturesExecute(object parametar)
        {
            Album SelectedAlbum = parametar as Album;

            Window albumPictures = new Window();
            albumPictures.Height = 500;
            albumPictures.Width = 700;
            albumPictures.Content = new ShowAlbumPicturesViewModel(this.Proxy, SelectedAlbum);
            albumPictures.Show();
            log.Debug("ShowAlbumPictures: Windows showed");
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
