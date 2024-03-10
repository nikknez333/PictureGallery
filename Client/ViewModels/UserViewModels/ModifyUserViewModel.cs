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

namespace Client.ViewModels.UserViewModels
{
    public class ModifyUserViewModel : INotifyPropertyChanged
    {
        public User User { get; set; }
        private static readonly ILog log = LogHelper.GetLogger();
        private string PreviousUsername { get; set; }
        private string PreviousFirstName { get; set; }
        private string PreviousLastName { get; set; }

        public ICommand SubmitModifyCommand { get; set; }
        private IServices Proxy { get; set; }

        public ModifyUserViewModel()
        {

        }

        public ModifyUserViewModel(User u, IServices proxy)
        {
            this.SubmitModifyCommand = new RelayCommand(SubmitModifyExecute, SubmitModifyCanExecute);

            this.User = u;

            this.PreviousUsername = this.User.Username;
            this.PreviousFirstName = this.User.Name;
            this.PreviousLastName = this.User.Surname;

            this.Proxy = proxy;
            log.Debug("ModifyUser: ViewModel constructor called.");
        }

        public bool SubmitModifyCanExecute(object parametar)
        {
            if (string.IsNullOrEmpty(User.Username) || string.IsNullOrEmpty(User.Name) || string.IsNullOrEmpty(User.Surname))
                return false;

            if (User.Username == PreviousUsername && User.Name == PreviousFirstName && User.Surname == PreviousLastName)
                return false;

            return true;
        }

        public void SubmitModifyExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            bool IsSuccessfullyExecuted = false;

            try
            {
                IsSuccessfullyExecuted = this.Proxy.ModifyUser(this.User, PreviousUsername);
            }
            catch(Exception e)
            {
                log.Error(String.Format("ModifyUser Server Error: {0}", e.Message));
            }

            if (IsSuccessfullyExecuted)
                log.Info("ModifyUser: Successfully executed");
            else
                log.Error("ModifyUser: Failed to execute");

            UserControl userControl = parametars[0] as UserControl;
            Window window = Window.GetWindow(userControl);
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
