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

namespace Client.ViewModels.EventViewModels
{
    public class ModifyEventViewModel : INotifyPropertyChanged
    {
        private static readonly ILog log = LogHelper.GetLogger();
        private IServices Proxy { get; set; }
        public Event SelectedEvent { get; set; }
        public ICommand SubmitModifyEventCommand { get; set; }

        public ModifyEventViewModel(Event selectedEvent, IServices proxy)
        {
            this.SubmitModifyEventCommand = new RelayCommand(SubmitModifyEventExecute, SubmitModifyCanExecute);
            this.SelectedEvent = selectedEvent;
            this.Proxy = proxy;

            log.Debug("ModifyEvent: ViewModel constructor called");
        }

        public bool SubmitModifyCanExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            String modifyEvtName = parametars[0] as String;
            DateTime modifyEvtDate = (DateTime)parametars[1];
            String modifyEvtLocation = parametars[2] as String;

            if (parametars == null || string.IsNullOrEmpty(modifyEvtName) || modifyEvtDate.Date == null || string.IsNullOrEmpty(modifyEvtLocation))
                return false;

            return true;
        }

        public void SubmitModifyEventExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            bool IsSuccessfullyExecuted = false;

            try
            {
                IsSuccessfullyExecuted = Proxy.ModifyEvent(SelectedEvent);
            }
            catch(Exception e)
            {
                log.Error(String.Format("ModifyEvent Server Error: {0}", e.Message));
            }

            if (IsSuccessfullyExecuted)
                log.Info("ModifyEvent: Successfully executed");
            else
                log.Error("ModifyEvent: Failed to execute");

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
