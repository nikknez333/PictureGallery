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
    public class AddNewUserViewModel : INotifyPropertyChanged
    {
        private static readonly ILog log = LogHelper.GetLogger();
        public User User { get; set; }
        public IServices Proxy { get; set; }
        public ICommand SubmitAddUserCommand { get; set; }

        public AddNewUserViewModel(IServices proxy, User u)
        {
            this.User = u;
            this.Proxy = proxy;
            this.SubmitAddUserCommand = new RelayCommand(SubmitAddUserExecute, SubmitAddUserCanExecute);
            log.Debug("AddNewUser: ViewModel constructor called");
        }

        public bool SubmitAddUserCanExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            String username = parametars[0] as String;
            String password = parametars[1] as String;
            String firstName = parametars[2] as String;
            String lastName = parametars[3] as String;

            if (parametars == null || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                return false;

            return true;

        }

        public void SubmitAddUserExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            String username = parametars[0] as String;
            String password = parametars[1] as String;
            String firstName = parametars[2] as String;
            String lastName = parametars[3] as String;
            Boolean isAdmin = (Boolean)parametars[4];

            User newUser = new User();

            newUser.Username = username;
            newUser.Password = password;
            newUser.Name = firstName;
            newUser.Surname = lastName;

            if (isAdmin)
                newUser.Type = TypeOfUser.ADMIN;
            else
                newUser.Type = TypeOfUser.USER;

            bool IsSuccessfullyExecuted = false;

            try
            {
                IsSuccessfullyExecuted = Proxy.AddNewUser(newUser);
            }
            catch(Exception e)
            {
                log.Error(String.Format("AddNewUser Server Error: {0}", e.Message));
            }

            if (IsSuccessfullyExecuted)
                log.Info("AddNewUser: Successfully executed");
            else
                log.Error("AddNewUser: Failed to execute");

            UserControl uc = parametars[5] as UserControl;

            Window Window = Window.GetWindow(uc);
            Window.Close();
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
