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
    public class AddNewAlbumViewModel : INotifyPropertyChanged
    {
        private static readonly ILog log = LogHelper.GetLogger();
        private IServices Proxy { get; set; }
        public ICommand SubmitAddAlbumCommand { get; set; }
        public ObservableCollection<Album> AllAlbums { get; set; }

        public User User { get; set; }

        public AddNewAlbumViewModel(User u, IServices proxy, ObservableCollection<Album> Albums)
        {
            this.User = u;
            this.SubmitAddAlbumCommand = new RelayCommand(SubmitAddAlbumExecute, SubmitAddAlbumCanExecute);
            this.Proxy = proxy;
            this.AllAlbums = Albums;
            log.Debug("AddNewAlbum: ViewModel constructor called");
        }

        public bool SubmitAddAlbumCanExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            String albumName = parametars[0] as String;
            DateTime? albumDate = (DateTime?)parametars[1];

            if (parametars == null || string.IsNullOrEmpty(albumName) || albumDate == null)
                return false;

            return true;
            
        }

        public void SubmitAddAlbumExecute(object parameter)
        {
            Object[] parametars = parameter as Object[];

            String albumName = parametars[0] as String;
            DateTime albumDate = (DateTime)parametars[1];
            Boolean isChecked = (Boolean)parametars[2];

            Album album = new Album();

            album.Name = albumName;
            album.Date = albumDate;
            album.IsPrivate = isChecked;
            album.CreatedByUser = User.Username;
            album.Pictures = null;

            Tuple<bool, int> result = new Tuple<bool, int>(false, 0);

            try
            {
                result = Proxy.AddNewAlbum(album);
            }
            catch(Exception e)
            {
                log.Error(String.Format("AddNewAlbum Server Error: {0}", e.Message));
            }

            if (result.Item1)
            {
                log.Info("AddNewAlbum: Successfully executed");
                album.AlbumID = result.Item2;
                this.AllAlbums.Add(album);
            }
            else
                log.Error("AddNewAlbum: Failed to execute");

            UserControl uc = parametars[3] as UserControl;

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
