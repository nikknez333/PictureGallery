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

namespace Client.ViewModels.EventViewModels
{
    public class ShowEventViewModel : INotifyPropertyChanged
    {
        private static readonly ILog log = LogHelper.GetLogger();
        private IServices Proxy { get; set; }
        public ICommand GoBackCommand { get; set; }
        public ICommand ModifyEventCommand { get; set; }
        public ICommand DeleteEventCommand { get; set; }
        public ObservableCollection<Event> AllEvents { get; set; }
        public User User { get; set; }

        public ShowEventViewModel(IServices proxy, User user, ObservableCollection<Event> Events)
        {
            this.GoBackCommand = new RelayCommand(GoBackCommandExecute, GoBackCommandCanExecute);
            this.ModifyEventCommand = new RelayCommand(ModifyEventExecute, ModifyEventCanExecute);
            this.DeleteEventCommand = new RelayCommand(DeleteEventExecute, DeleteEventCanExecute);
            AllEvents = Events;
            this.User = user;
            this.Proxy = proxy;

            log.Debug("ShowEvent: ViewModel constructor called");
        }

        public bool GoBackCommandCanExecute(object parametar)
        {
            return true;
        }

        public void GoBackCommandExecute(object parametar)
        {
            UserControl uc = parametar as UserControl;

            Window Window = Window.GetWindow(uc);
            log.Info("GoBack: Successfully executed");
            Window.Close();
        }

        public bool ModifyEventCanExecute(object parametar)
        {
            Event selectedEvent = parametar as Event;

            if (parametar == null || selectedEvent == null)
                return false;

            return true;
        }

        public void ModifyEventExecute(object parametar)
        {
            Event SelectedEvent = parametar as Event;

            Window modifyEvent = new Window();
            modifyEvent.Height = 550;
            modifyEvent.Width = 450;
            modifyEvent.Content = new ModifyEventViewModel(SelectedEvent, Proxy);
            modifyEvent.ShowDialog();
            log.Debug("ModifyEvent: Window showed");
        }

        public bool DeleteEventCanExecute(object parametar)
        {
            Event selectedEvent = parametar as Event;

            if (parametar == null || selectedEvent == null)
                return false;
            return true;
        }
        
        public void DeleteEventExecute(object parametar)
        {
            Event selectedEvent = parametar as Event;

            bool IsSuccessfullyExecuted = false;

            try
            {
                IsSuccessfullyExecuted = Proxy.DeleteEvent(selectedEvent);
            }
            catch(Exception e)
            {
                log.Error(String.Format("DeleteEvent Server Error: {0}", e.Message));
            }

            if (IsSuccessfullyExecuted)
                log.Info("DeleteEvent: Successfully executed");
            else
                log.Error("DeleteEvent: Failed to execute");

            AllEvents.Remove(selectedEvent);
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
