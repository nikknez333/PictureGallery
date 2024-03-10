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
    public class ShowAlbumPicturesViewModel : INotifyPropertyChanged
    {
        private static readonly ILog log = LogHelper.GetLogger();

        private IServices Proxy { get; set; }
        public ObservableCollection<Picture> AlbumPictures { get; set; }
        public ICommand DeletePhotoAlbumCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public Album SelectedAlbum { get; set; }


        public ShowAlbumPicturesViewModel(IServices proxy, Album SelectedAlbum)
        {
            this.Proxy = proxy;
            this.DeletePhotoAlbumCommand = new RelayCommand(DeletePhotoAlbumExecute, DeletePhotoAlbumCanExecute);
            this.GoBackCommand = new RelayCommand(GoBackExecute, GoBackCanExecute);
            this.AlbumPictures = new ObservableCollection<Picture>(Proxy.GetAlbumPictures(SelectedAlbum));
            this.SelectedAlbum = SelectedAlbum;

            log.Debug("ShowAlbumPictures: ViewModel constructor called");
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

        public bool DeletePhotoAlbumCanExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            Picture SelectedPicture = parametars[0] as Picture;

            if (SelectedPicture == null)
                return false;

            return true;
        }

        public void DeletePhotoAlbumExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            Picture SelectedPicture = parametars[0] as Picture;

            bool IsSuccessfullyExecuted = false;

            try
            {
                IsSuccessfullyExecuted = Proxy.DeletePhotoFromAlbum(SelectedAlbum, SelectedPicture);
            }
            catch(Exception e)
            {
                log.Error(String.Format("DeletePhotoFromAlbum Server Error: {0}", e.Message));
            }

            if (IsSuccessfullyExecuted)
                log.Info("DeltePhotoFromAlbum: Successfully executed");
            else
                log.Error("DeletePhotoFromAlbum: Failed to execute");

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
