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
    public class RatePhotoViewModel : INotifyPropertyChanged
    {
        private static readonly ILog log = LogHelper.GetLogger();
        public User User { get; set; }
        private ObservableCollection<Picture> allPictures;
        public ObservableCollection<Picture> AllPictures
        {
            get
            {
                return allPictures;
            }
            set
            {
                if (allPictures == value)
                    return;

                allPictures = value;
                OnPropertyChanged(nameof(allPictures));
            }
        }

        private IServices Proxy { get; set; }

        public ICommand RatePhoto1Command { get; set; }
        public ICommand RatePhoto2Command { get; set; }
        public ICommand RatePhoto3Command { get; set; }
        public ICommand RatePhoto4Command { get; set; }
        public ICommand RatePhoto5Command { get; set; }
        public ICommand GoBackCommand { get; set; }

        public RatePhotoViewModel(User u, IServices proxy, ObservableCollection<Picture> AllPictures)
        {
            this.User = u;
            this.Proxy = proxy;
            this.AllPictures = AllPictures;

            this.GoBackCommand = new RelayCommand(GoBackExecute, GoBackCanExecute);
            this.RatePhoto1Command = new RelayCommand(RatePhoto1Execute, RatePhoto1CanExecute);
            this.RatePhoto2Command = new RelayCommand(RatePhoto2Execute, RatePhoto2CanExecute);
            this.RatePhoto3Command = new RelayCommand(RatePhoto3Execute, RatePhoto3CanExecute);
            this.RatePhoto4Command = new RelayCommand(RatePhoto4Execute, RatePhoto4CanExecute);
            this.RatePhoto5Command = new RelayCommand(RatePhoto5Execute, RatePhoto5CanExecute);
            log.Debug("RatePhoto: ViewModel constructor called");
        }

        public bool GoBackCanExecute(object parametar)
        {
            return true;
        }

        public void GoBackExecute(object parametar)
        {
            UserControl uc = parametar as UserControl;

            Window window = Window.GetWindow(uc);
            window.Close();
        }

        public bool RatePhoto1CanExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            Picture selectedPicture = parametars[0] as Picture;

            if (selectedPicture == null || User.ratedPictures.Contains(selectedPicture.PictureID))
                return false;
            
            return true;
        }

        public void RatePhoto1Execute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            Picture selectedPicture = parametars[0] as Picture;

            double returnedRating = 0;

            try
            {
                returnedRating = Proxy.RatePhoto(selectedPicture, 1, User);
            }
            catch(Exception e)
            {
                log.Error(String.Format("RatePhoto Server Error: {0}", e.Message));
            }

            if (returnedRating <= 0)
            {
                log.Info("RatePhoto1: Successfully executed");
                AllPictures.FirstOrDefault(pic => pic.PictureID == selectedPicture.PictureID).Rating = returnedRating;
            }
            else
                log.Error("RatePhoto1: Failed to execute");
        }

        public bool RatePhoto2CanExecute(object parametar)
        {
            
            Object[] parametars = parametar as Object[];

            Picture selectedPicture = parametars[0] as Picture;

            if (selectedPicture == null || User.ratedPictures.Contains(selectedPicture.PictureID))
                return false;
                
            return true;
        }

        public void RatePhoto2Execute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            Picture selectedPicture = parametars[0] as Picture;

            double returnedRating = 0;

            try
            {
                returnedRating = Proxy.RatePhoto(selectedPicture, 2, User);
            }
            catch (Exception e)
            {
                log.Error(String.Format("RatePhoto2 Server Error: {0}", e.Message));
            }

            if (returnedRating <= 0)
            {
                log.Info("RatePhoto2: Successfully executed");
                Picture p = AllPictures.FirstOrDefault(pic => pic.PictureID == selectedPicture.PictureID);
                p.Rating = returnedRating;
            }
            else
                log.Error("RatePhoto2: Failed to execute");
        }

        public bool RatePhoto3CanExecute(object parametar)
        {
            
            Object[] parametars = parametar as Object[];

            Picture selectedPicture = parametars[0] as Picture;

            if (selectedPicture == null || User.ratedPictures.Contains(selectedPicture.PictureID))
                return false;
            
            return true;
        }

        public void RatePhoto3Execute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            Picture selectedPicture = parametars[0] as Picture;

            double returnedRating = 0;

            try
            {
                returnedRating = Proxy.RatePhoto(selectedPicture, 3, User);
            }
            catch (Exception e)
            {
                log.Error(String.Format("RatePhoto3 Server Error: {0}", e.Message));
            }

            if (returnedRating <= 0)
            {
                log.Info("RatePhoto3: Successfully executed");
                Picture p = AllPictures.FirstOrDefault(pic => pic.PictureID == selectedPicture.PictureID);
                p.Rating = returnedRating;
            }
            else
                log.Error("RatePhoto3: Failed to execute");
        }

        public bool RatePhoto4CanExecute(object parametar)
        {
            
            Object[] parametars = parametar as Object[];

            Picture selectedPicture = parametars[0] as Picture;

            if (selectedPicture == null || User.ratedPictures.Contains(selectedPicture.PictureID))
                return false;
                
            return true;
        }

        public void RatePhoto4Execute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            Picture selectedPicture = parametars[0] as Picture;

            double returnedRating = 0;

            try
            {
                returnedRating = Proxy.RatePhoto(selectedPicture, 4, User);
            }
            catch (Exception e)
            {
                log.Error(String.Format("RatePhoto4 Server Error: {0}", e.Message));
            }

            if (returnedRating <= 0)
            {
                log.Info("RatePhoto4: Successfully executed");
                Picture p = AllPictures.FirstOrDefault(pic => pic.PictureID == selectedPicture.PictureID);
                p.Rating = returnedRating;
            }
            else
                log.Error("RatePhoto4: Failed to execute");
        }

        public bool RatePhoto5CanExecute(object parametar)
        {
            
            Object[] parametars = parametar as Object[];

            Picture selectedPicture = parametars[0] as Picture;

            if (selectedPicture == null || User.ratedPictures.Contains(selectedPicture.PictureID))
                return false;
                
            return true;
        }

        public void RatePhoto5Execute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            Picture selectedPicture = parametars[0] as Picture;

            double returnedRating = 0;

            try
            {
                returnedRating = Proxy.RatePhoto(selectedPicture, 5, User);
            }
            catch (Exception e)
            {
                log.Error(String.Format("RatePhoto2 Server Error: {0}", e.Message));
            }

            if (returnedRating <= 0)
            {
                log.Info("RatePhoto5: Successfully executed");
                Picture p = AllPictures.FirstOrDefault(pic => pic.PictureID == selectedPicture.PictureID);
                p.Rating = returnedRating;
            }
            else
                log.Error("RatePhoto5: Failed to execute");
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
