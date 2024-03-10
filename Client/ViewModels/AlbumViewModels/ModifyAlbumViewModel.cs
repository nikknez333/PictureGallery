using Client.Helpers;
using Common.Contract;
using Common.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.ViewModels.AlbumViewModels
{
    public class ModifyAlbumViewModel : INotifyPropertyChanged
    {
        private static readonly ILog log = LogHelper.GetLogger();
        private IServices Proxy { get; set; }
        public Album SelectedAlbum { get; set; }
        public ICommand SubmitModifyAlbumCommand { get; set; }

        public ModifyAlbumViewModel(Album selectedAlbum, IServices proxy)
        {
            this.SubmitModifyAlbumCommand = new RelayCommand(SubmitModifyAlbumExecute, SubmitModifyAlbumCanExecute);
            this.SelectedAlbum = selectedAlbum;
            this.Proxy = proxy;

            log.Debug("ModifyAlbum: ViewModel constructor called");
        }

        public bool SubmitModifyAlbumCanExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            String modifyAlbName = parametars[0] as String;
            DateTime modifyAlbDate = (DateTime)parametars[1];

            if (parametars == null || string.IsNullOrEmpty(modifyAlbName) || modifyAlbDate.Date == null)
                return false;

            return true;
        }

        public void SubmitModifyAlbumExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            SelectedAlbum.IsPrivate = (Boolean)parametars[2];

            bool IsSuccessfullyExecuted = false;

            try
            {
                IsSuccessfullyExecuted = Proxy.ModifyAlbum(SelectedAlbum);
            }
            catch(Exception e)
            {
                log.Error(String.Format("ModifyAlbum Server Error: {0}", e.Message));
            }

            if (IsSuccessfullyExecuted)
                log.Info("ModifyAlbum: Successfully executed");
            else
                log.Error("ModifyAlbum: Failed to execute");

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
