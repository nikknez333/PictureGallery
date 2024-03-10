using Client.CommandPattern;
using Client.CommandPattern.ConcretePictureCommands;
using Client.Helpers;
using Client.ViewModels.AlbumViewModels;
using Client.ViewModels.EventViewModels;
using Client.ViewModels.PictureViewModels;
using Client.ViewModels.UserViewModels;
using Common.Contract;
using Common.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Client.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged, IInvoker
    {
        public User User { get; set; }
        private static readonly ILog log = LogHelper.GetLogger();
        private ObservableCollection<Picture> pictures;
        private ObservableCollection<Album> albums;
        private ObservableCollection<Event> events;

        public ObservableCollection<Picture> Pictures
        {
            get
            {
                return pictures;
            }
            set
            {
                if (pictures == value)
                    return;

                pictures = value;
                OnPropertyChanged(nameof(Pictures));
            }
        }
        public ObservableCollection<Album> Albums
        {
            get
            {
                return albums;
            }
            set
            {
                if (albums == value)
                    return;

                albums = value;
                OnPropertyChanged(nameof(Albums));
            }
        }

        public ObservableCollection<Event> Events
        {
            get
            {
                return events;
            }
            set
            {
                if (events == value)
                    return;

                events = value;
                OnPropertyChanged(nameof(Event));
            }
        }

        private IServices proxy;



        #region HomePageCommands
        public ICommand EditProfileCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand ShowAlbumsCommand { get; set; }
        public ICommand ShowMyEventsCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand UndoCommand { get; set; }
        public ICommand RedoCommand { get; set; }
        public ICommand AddNewUserCommand { get; set; }
        public ICommand AddNewPhotoCommand { get; set; }
        public ICommand ModifyPhotoCommand { get; set; }
        public ICommand DeletePhotoCommand { get; set; }
        public ICommand DuplicatePhotoCommand { get; set; }
        public ICommand AddNewAlbumCommand { get; set; }
        public ICommand AddPhotoAlbumCommand { get; set; }
        public ICommand DeletePhotoAlbumCommand { get; set; }
        public ICommand AddNewEventCommand { get; set; }
        public ICommand ConnEventAlbumCommand { get; set; }
        public ICommand RemoveEventAlbCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public ICommand RatePhotoCommand { get; set; }
        #endregion

        public List<Command> CommandsList { get; set; }
        public int currentCommand { get; set; }

        public HomePageViewModel(User u)
        {
            this.EditProfileCommand = new RelayCommand(EditProfileExecute, EditProfileCanExecute);
            this.RefreshCommand = new RelayCommand(RefreshExecute, RefreshCanExecute);
            this.ShowAlbumsCommand = new RelayCommand(ShowAlbumsExecute, ShowAlbumsCanExecute);
            this.ShowMyEventsCommand = new RelayCommand(ShowEventsExecute, ShowEventsCanExecute);
            this.SearchCommand = new RelayCommand(SearchExecute, SearchCanExecute);
            this.UndoCommand = new RelayCommand(UndoExecute, UndoCanExecute);
            this.RedoCommand = new RelayCommand(RedoExecute, RedoCanExecute);
            this.AddNewUserCommand = new RelayCommand(AddNewUserExecute, AddNewUserCanExecute);
            this.AddNewPhotoCommand = new RelayCommand(AddNewPhotoExecute, AddNewPhotoCanExecute);
            this.ModifyPhotoCommand = new RelayCommand(ModifyPhotoExecute, ModifyPhotoCanExecute);
            this.DeletePhotoCommand = new RelayCommand(DeletePhotoExecute, DeletePhotoCanExecute);
            this.DuplicatePhotoCommand = new RelayCommand(DuplicatePhotoExecute, DuplicatePhotoCanExecute);
            this.AddNewAlbumCommand = new RelayCommand(AddNewAlbumExecute, AddNewAlbumCanExecute);
            this.AddPhotoAlbumCommand = new RelayCommand(AddPhotoAlbumExecute, AddPhotoAlbumCanExecute);
            this.DeletePhotoAlbumCommand = new RelayCommand(DeletePhotoAlbumExecute, DeletePhotoAlbumCanExecute);
            this.AddNewEventCommand = new RelayCommand(AddNewEventExecute, AddNewEventCanExecute);
            this.ConnEventAlbumCommand = new RelayCommand(ConnEventAlbumExecute, ConnEventAlbumCanExecute);
            this.RemoveEventAlbCommand = new RelayCommand(RemoveEventAlbExecute, RemoveEventAlbCanExecute);
            this.LogOutCommand = new RelayCommand(LogOutExecute, LogOutCanExecute);
            this.RatePhotoCommand = new RelayCommand(RateExecute, RateCanExecute);
            this.User = u;

            CommandsList = new List<Command>();
            currentCommand = 0;
            NetTcpBinding binding = new NetTcpBinding()
            {
                MaxReceivedMessageSize = 20000000,
            };
            ChannelFactory<IServices> factory = new ChannelFactory<IServices>(binding, new EndpointAddress("net.tcp://localhost:8999/IServices"));
            proxy = factory.CreateChannel();

            this.Pictures = new ObservableCollection<Picture>(proxy.GetAllPhotos(this.User));
            this.Albums = new ObservableCollection<Album>(proxy.GetAllAlbums(this.User));
            this.Events = new ObservableCollection<Event>(proxy.GetAllEvents());

            log.Debug("Homepage: ViewModel constructor called");
        }

        public bool RateCanExecute(object parametar)
        {
            return true;
        }

        public void RateExecute(object parametar)
        {
            Window rateWindow = new Window();
            rateWindow.Height = 500;
            rateWindow.Width = 700;
            rateWindow.Content = new RatePhotoViewModel(User, proxy, Pictures);
            rateWindow.ShowDialog();
        }

        public bool LogOutCanExecute(object parametar)
        {
            return true;
        }

        public void LogOutExecute(object parametar)
        {
            Window loginWindow = new Window();
            loginWindow.Height = 700;
            loginWindow.Width = 620;
            loginWindow.Content = new LoginViewModel();
            loginWindow.Show();

            UserControl uc = parametar as UserControl;

            Window Window = Window.GetWindow(uc);
            log.Info(String.Format(String.Format("Logout: Success(User{0})", this.User.Username)));
            Window.Close();
        }

        public bool RefreshCanExecute(object parametar)
        {
            return true;
        }

        public void RefreshExecute(object parametar)
        {
            ObservableCollection<Picture> refreshedPictures = new ObservableCollection<Picture>(proxy.GetAllPhotos(this.User));
            ObservableCollection<Album> refreshedAlbums = new ObservableCollection<Album>(proxy.GetAllAlbums(this.User));
            ObservableCollection<Event> refreshedEvents = new ObservableCollection<Event>(proxy.GetAllEvents());
            Pictures = refreshedPictures;
            Albums = refreshedAlbums;
            Events = refreshedEvents;
            log.Info("Refresh: Successfully executed");

        }

        public bool EditProfileCanExecute(object parametar)
        {
            return true;
        }

        public void EditProfileExecute(object parametar)
        {
            Window modifyProfileWindow = new Window();
            modifyProfileWindow.Height = 570;
            modifyProfileWindow.Width = 400;
            modifyProfileWindow.Content = new ModifyUserViewModel(User, proxy);
            modifyProfileWindow.ShowDialog();
            log.Debug("EditProfile: Dialog showed");
        }

        public bool AddNewUserCanExecute(object parametar)
        {
            return this.User.Type == TypeOfUser.ADMIN;
        }

        public void AddNewUserExecute(object parametar)
        {
            Window addNewUserWindow = new Window();
            addNewUserWindow.Height = 670;
            addNewUserWindow.Width = 400;
            addNewUserWindow.Content = new AddNewUserViewModel(proxy, User);
            addNewUserWindow.ShowDialog();
            log.Debug("AddNewUser: Dialog showed");
        }

        public bool AddNewPhotoCanExecute(object parametar)
        {
            return true;
        }

        public void AddNewPhotoExecute(object parametar)
        {
            Window addNewPhoto = new Window();
            addNewPhoto.Height = 920;
            addNewPhoto.Width = 450;
            addNewPhoto.Content = new AddNewPhotoViewModel(Pictures,proxy, User, this);
            addNewPhoto.ShowDialog();
            log.Debug("AddNewPhoto: Dialog showed");
        }

        public bool ModifyPhotoCanExecute(object parametar)
        {
            Picture SelectedPicture = parametar as Picture;

            if (parametar == null || SelectedPicture == null)
                return false;

            return true;
        }

        public void ModifyPhotoExecute(object parametar)
        {
            Picture SelectedPicture = parametar as Picture;

            Window modifyPhoto = new Window();
            modifyPhoto.Height = 920;
            modifyPhoto.Width = 450;
            modifyPhoto.Content = new ModifyPhotoViewModel(SelectedPicture, proxy, this, Pictures);
            modifyPhoto.ShowDialog();
            log.Debug("ModifyPhoto: Dialog showed");
        }

        public bool DeletePhotoCanExecute(object parametar)
        {
            Picture SelectedPicture = parametar as Picture;

            if (parametar == null || SelectedPicture == null)
                return false;

            return true;
        }

        public void DeletePhotoExecute(object parametar)
        {
            Picture photoToDelete = parametar as Picture;

            
            Picture removedPicture = null;
            try
            {
               removedPicture = proxy.DeletePhoto(photoToDelete);
               CommandRemovePicture commandRemovePicture = new CommandRemovePicture(removedPicture, new ServerReceiver(), proxy, Pictures);
               RememberCommand(commandRemovePicture);
            }
            catch(Exception e)
            {
                log.Error(String.Format("DeletePhoto Server Error: {0}", e.Message));
            }

            if (removedPicture != null)
                log.Info("DeletPhoto: Successfully executed");
            else
                log.Error("DeletePhoto: Failed to execute");
            Pictures.Remove(photoToDelete);
        }

        public bool DuplicatePhotoCanExecute(object parametar)
        {
            Picture SelectedPicture = parametar as Picture;

            if (parametar == null || SelectedPicture == null)
                return false;

            return true;
        }

        public void DuplicatePhotoExecute(object parametar)
        {
            Picture SelectedPicture = parametar as Picture;

            Tuple<bool, int, Picture> result = new Tuple<bool, int, Picture>(false, 0, new Picture());

            Picture duplicatePicture = (Picture)SelectedPicture.Clone();
            try
            {
                result = proxy.AddNewPhoto(duplicatePicture);
                if (result.Item3 != null || result.Item3.PictureID != 0)
                {
                    CommandDuplicatePicture commandDuplicatePicture = new CommandDuplicatePicture(duplicatePicture, new ServerReceiver(), proxy, Pictures);
                    RememberCommand(commandDuplicatePicture);
                }
            }
            catch(Exception e)
            {
                log.Error(String.Format("DuplicatePhoto Server Error: {0}", e.Message));
            }

            if (result.Item1)
                log.Info("DuplicatePhoto: Successfully exectued");
            else
                log.Error("DuplicatePhoto: Failed to execute");
            Pictures.Add(duplicatePicture);
        }

        public bool AddNewAlbumCanExecute(object parametar)
        {
            return true;
        }

        public void AddNewAlbumExecute(object parametar)
        {
            Window addAlbumWindow = new Window();
            addAlbumWindow.Height = 600;
            addAlbumWindow.Width = 450;
            addAlbumWindow.Content = new AddNewAlbumViewModel(User, proxy, Albums);
            addAlbumWindow.ShowDialog();
            log.Debug("AddNewAlbum: Window showed");
        }

        public bool AddPhotoAlbumCanExecute(object parametar)
        {
            
           /*if (Pictures.Count != 0 && Albums.Count != 0)
                return false;*/

            return true;
        }

        public void AddPhotoAlbumExecute(object parametar)
        {
            Window addPhotoToAlbum = new Window();
            addPhotoToAlbum.Height = 1000;
            addPhotoToAlbum.Width = 700;
            addPhotoToAlbum.Content = new AddPhotoAlbumViewModel(proxy, User, Pictures, Albums);
            addPhotoToAlbum.ShowDialog();
            log.Debug("AddPhotoAlbum: Window showed");
        }

        public bool DeletePhotoAlbumCanExecute(object parametar)
        {
            /*if (MyPictures.Count == 0 && MyAlbums.Count == 0)
                return false;*/

            return true;
        }

        public void DeletePhotoAlbumExecute(object parametar)
        {
            Window deletePhotoToAlbum = new Window();
            deletePhotoToAlbum.Height = 750;
            deletePhotoToAlbum.Width = 750;
            deletePhotoToAlbum.Content = new DeletePhotoAlbumViewModel(proxy, User, Albums);
            deletePhotoToAlbum.ShowDialog();
            log.Debug("DeletePhotoAlbum: Window showed");
        }

        public bool ConnEventAlbumCanExecute(object parametar)
        {
            /*if (MyEvents.Count == 0 && MyAlbums.Count == 0)
                return false;*/

            return true;
        }

        public void ConnEventAlbumExecute(object parametar)
        {
            Window connPhotoToEvent = new Window();
            connPhotoToEvent.Height = 1000;
            connPhotoToEvent.Width = 700;
            connPhotoToEvent.Content = new ConnectEventAlbumViewModel(proxy, User, Events, Albums);
            connPhotoToEvent.ShowDialog();
            log.Debug("ConnEventAlbum: Window showed");
        }

        public bool RemoveEventAlbCanExecute(object parametar)
        {
           /* if (MyEvents.Count == 0 && MyAlbums.Count == 0)
                return false;*/

            return true;
        }
        
        public void RemoveEventAlbExecute(object parametar)
        {
            Window removePhotoFromEvent = new Window();
            removePhotoFromEvent.Height = 700;
            removePhotoFromEvent.Width = 700;
            removePhotoFromEvent.Content = new RemoveEventAlbumViewModel(proxy, User, Events, Albums);
            removePhotoFromEvent.ShowDialog();
            log.Debug("RemoveEventAlb: Window showed");
        }

        public bool AddNewEventCanExecute(object parametar)
        {
            return true;
        }

        public void AddNewEventExecute(object parametar)
        {
            Window addNewEventWindow = new Window();
            addNewEventWindow.Height = 570;
            addNewEventWindow.Width = 400;
            addNewEventWindow.Content = new AddNewEventViewModel(proxy, User, Events);
            addNewEventWindow.ShowDialog();
            log.Debug("AddNewEvent: Window showed");
        }
        

        public bool ShowPhotosCanExecute(object parametar)
        {
            return true;
        }

        public void ShowPhotosExecute(object parametar)
        {
            if (this.Pictures != null)
                try
                {
                    this.Pictures = new ObservableCollection<Picture>(proxy.GetAllPhotos(this.User));
                }
                catch(Exception e)
                {
                    log.Error(String.Format("ShowPhotos Server Error: {0}", e.Message));
                }
                log.Info("ShowPhotos: Successfully executed");
        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        public bool ShowAlbumsCanExecute(object parametar)
        {
            return true;
        }

        public void ShowAlbumsExecute(object parametar)
        {
            Window showAllAlbums = new Window();
            showAllAlbums.Height = 750;
            showAllAlbums.Width = 550;
            showAllAlbums.Content = new ShowAlbumViewModel(User, proxy, Albums);
            showAllAlbums.ShowDialog();
            log.Debug("ShowAlbums: Window showed");
        }

        public bool ShowEventsCanExecute(object parametar)
        {
            return true;
        }

        public void ShowEventsExecute(object parametar)
        {
            Window showAllEvents = new Window();
            showAllEvents.Height = 750;
            showAllEvents.Width = 550;
            showAllEvents.Content = new ShowEventViewModel(proxy,User, Events);
            showAllEvents.ShowDialog();
            log.Debug("ShowEvents: Window showed");
        }

        public bool SearchCanExecute(object parametar)
        {
            return true;
        }

        public void SearchExecute(object parametar)
        {
            Object[] parametars = parametar as Object[];

            Pictures.Clear();

            var pictures = new ObservableCollection<Picture>(proxy.GetAllPhotos(User));

            foreach(Picture pic in pictures)
            {
                Pictures.Add(pic);
            }
          
            if (string.IsNullOrEmpty((String)parametars[0]) && string.IsNullOrEmpty((String)parametars[1]) && parametars[2] == null && parametars[3] == null)
                return;

            String photoNameSearch;
            String photoTagsSearch;

            DateTime? photoCreatedFrom = null;
            DateTime? photoCreatedTo = null;

            
            if (parametars[0] as String == String.Empty)
                photoNameSearch = "";
            else
                photoNameSearch = parametars[0] as String;

            if (parametars[1] as String == String.Empty)
                photoTagsSearch = "";
            else
                photoTagsSearch = parametars[1] as String;

            if (parametars[2] == null)
                photoCreatedFrom = DateTime.MinValue;
            else
                photoCreatedFrom = (DateTime)parametars[2];

            if (parametars[3] == null)
                photoCreatedTo = DateTime.MaxValue;
            else
                photoCreatedTo = (DateTime)parametars[3];
         

            foreach(Picture pic in Pictures.ToList())
            {
                if (!pic.Name.Contains(photoNameSearch))
                    Pictures.Remove(pic);

                if (!pic.Tags.Contains(photoTagsSearch))
                    Pictures.Remove(pic);

                if (pic.Name.Contains(photoNameSearch) && pic.Tags.Contains(photoTagsSearch) && (pic.Date == null || pic.Date >= photoCreatedFrom && pic.Date <= photoCreatedTo))
                {
                    continue;
                }
                else
                {
                    Pictures.Remove(pic);
                }
            }
        }
        #region UndoRedo

        private bool UndoCanExecute(object parameter)
        {
            if (currentCommand == 0)
            {
                return false;
            }
            return true;
        }
        private void UndoExecute(object parameter)
        {
            Undo();
        }

        private bool RedoCanExecute(object parameter)
        {
            if (currentCommand == CommandsList.Count)
            {
                return false;
            }
            return true;
        }
        private void RedoExecute(object parameter)
        {
            Redo();
        }

        public bool Undo()
        {
            if (currentCommand == 0)
            {
                return false;
            }
            Command Command = CommandsList[--currentCommand];
            Command.UnExecute();
            return true;
        }

        public bool Redo()
        {
            if (currentCommand == CommandsList.Count)
            {
                return false;
            }
            Command Command = CommandsList[currentCommand++];
            Command.Execute();
            return true;
        }

        public void RememberCommand(Command commandToAdd)
        {
            if (CommandsList.Count == 0 || currentCommand == CommandsList.Count)
            {
                CommandsList.Add(commandToAdd);
                currentCommand++;
            }
            else
            {
                CommandsList[currentCommand++] = commandToAdd;
            }

            if (currentCommand < CommandsList.Count)
            {
                CommandsList.RemoveRange(currentCommand, CommandsList.Count - currentCommand);
            }
        }
        #endregion

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
