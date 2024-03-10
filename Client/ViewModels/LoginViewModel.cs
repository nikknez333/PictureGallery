using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Common.Model;
using Client.Helpers;
using System.Windows;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Client.ViewModels
{
    public class LoginViewModel
    {
        private static readonly ILog log = LogHelper.GetLogger();
        public User User { get; set; }
        public UserControl UserControl { get; set; }
        public ICommand LoginCommand { get; set; }
        public LoginViewModel()
        {
            this.LoginCommand = new RelayCommand(Execute, CanExecute);
            this.User = new User();
        }

        private void Execute(object parametar)
        {
            UserControl UserControl = parametar as UserControl;

            try
            {
                User user = Proxy.Instance().proxyServices.Login(User.Username, User.Password);
                if(user == null)
                {
                    log.Info("Login: Invalid username or password");
                    MessageBox.Show("Invalid username or password");
                }
                else
                {
                    log.Info(String.Format("Login: Success(User:{0})",User.Username));
                    Window homePage = new Window();
                    homePage.Height = 670;
                    homePage.Width = 1100;
                    homePage.Content = new HomePageViewModel(user);
                    homePage.Show();
                    Window currentWindow = Window.GetWindow(UserControl);
                   currentWindow.Close();
                }
            }
            catch(Exception e)
            {
                log.Error(String.Format("Login Error: {0}", e.Message));
                MessageBox.Show($"Can't connect to server.Error{ e.Message}");
            }

        }

        private bool CanExecute(object parametar)
        {
            if(string.IsNullOrEmpty(this.User.Username) || string.IsNullOrEmpty(this.User.Password))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
