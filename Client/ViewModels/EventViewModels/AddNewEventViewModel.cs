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
    public class AddNewEventViewModel : INotifyPropertyChanged
    {
        private static readonly ILog log = LogHelper.GetLogger();
        private IServices Proxy { get; set; }
        public User User { get; set; }
        public ObservableCollection<Event> AllEvents { get; set; }
        public ICommand SubmitAddEventCommand { get; set; }

        public AddNewEventViewModel(IServices proxy, User u, ObservableCollection<Event> Events)
        {
            this.Proxy = proxy;
            this.SubmitAddEventCommand = new RelayCommand(SubmitAddEventExecute, SubmitAddEventCanExecute);
            this.AllEvents = Events;
            this.User = u;
            log.Debug("AddNewEvent: ViewModel constructor called");
        }

        public bool SubmitAddEventCanExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            String eventName = parametars[0] as String;
            DateTime? eventDate = (DateTime?)parametars[1];
            String eventLocation = parametars[2] as String;

            if (string.IsNullOrEmpty(eventName) || eventDate == null || string.IsNullOrEmpty(eventLocation))
                return false;

            return true;
        }

        public void SubmitAddEventExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            String eventName = parametars[0] as String;
            DateTime eventDate = (DateTime)parametars[1];
            String eventLocation = parametars[2] as String;

            Event Event = new Event();

            Event.Name = eventName;
            Event.Date = eventDate;
            Event.Location = eventLocation;
            Event.Albums = null;

            Tuple<bool, int> result = new Tuple<bool, int>(false, 0);

            try
            {
                result = Proxy.AddNewEvent(Event);
            }
            catch(Exception e)
            {
                log.Error(String.Format("AddNewEvent Server Error: {0}", e.Message));
            }

            if (result.Item1)
                log.Info("AddNewEvent: Successfully executed");
                
            else
                log.Error("AddNewEvent: Failed to execute");

            Event.EventID = result.Item2;
            AllEvents.Add(Event);
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
