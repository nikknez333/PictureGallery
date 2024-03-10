using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contract;
using Common.Model;
using log4net;
using Server.Access;

namespace Server
{
    public class Services : IServices
    {
        private static readonly Object lockObj = new object();
        private static readonly ILog log = LogHelper.GetLogger();

        public bool ModifyUser(User modifiedUser, string usernameToModify)
        {
            try
            {
                bool result = false;
                User getUser = null;

                lock(lockObj)
                {
                    getUser = DBManager.Instance.GetUserByUsername(modifiedUser.Username);
                }

                if (getUser == null)
                {
                    log.Warn("Conflict: user does not exists");
                    return false;
                }
                else
                {
                    lock (lockObj)
                    {
                        result = DBManager.Instance.ModifyUser(modifiedUser, usernameToModify);
                    }

                    log.Info("User: Successfully modified");
                    return result;
                }

            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to modify User, Error: {0}", e.Message));
                return false;
            }      
        }

        public bool AddNewUser(User UserToBeAdded)
        {
            try
            {
                bool result = false;
                User getUser = null;

                lock (lockObj)
                {
                    getUser = DBManager.Instance.GetUserByUsername(UserToBeAdded.Username);
                }

                if(getUser != null)
                {
                    log.Warn("Conflict: User already exists");
                    result = false;
                }

                lock (lockObj)
                {
                    result = DBManager.Instance.AddUser(UserToBeAdded);
                }

                log.Info("User: Successfully added");
                return result;
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to add User, Error: {0}", e.Message));
                return false;
            }  
        }

        public IEnumerable<Picture> GetAllPhotos(User LoggedInUser)
        {
            try
            {
                IEnumerable<Picture> photos;
                lock (lockObj)
                {
                    photos = DBManager.Instance.RetrieveAllPhotos(LoggedInUser);
                }
                log.Info("Get all photos: Successfully retreived");
                return photos;
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to get all photos, Error: {0}", e.Message));
                return null;
            }
        }


        public IEnumerable<Album> GetAllAlbums(User userAlbums)
        {
            try
            {
                IEnumerable<Album> albums;

                lock (lockObj)
                {
                    albums = DBManager.Instance.RetrieveAllAlbums(userAlbums);
                }
                log.Info("Get all albums: Successfully retreived");
                return albums;
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to get all albums, Error: {0}", e.Message));
                return null;
            }
        }

        public IEnumerable<Album> GetNonPrivateAlbums()
        {
            try
            {
                IEnumerable<Album> nonPrivateAlbums;

                lock (lockObj)
                {
                    nonPrivateAlbums = DBManager.Instance.RetrieveAllNonPrivateAlbums();
                }
                log.Info("Get non private albums: Successfully retreived");
                return nonPrivateAlbums;
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to get non private albums, Error: {0}", e.Message));
                return null;
            }
        }

        public IEnumerable<Event> GetAllEvents()
        {
            try
            {
                IEnumerable<Event> events;

                lock (lockObj)
                {
                    events = DBManager.Instance.RetrieveAllEvents();
                }
                log.Info("Get all events: Successfully retrieved");
                return events;
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to get all events, Error: {0}", e.Message));
                return null;
            }
        }

        public IEnumerable<Picture> GetAlbumPictures(Album album)
        {
            try
            {
                IEnumerable<Picture> albumPictures;

                lock (lockObj)
                {
                    albumPictures = DBManager.Instance.RetrieveAlbumPictures(album);
                }
                log.Info("Get album pictures: Successfully retrieved");
                return albumPictures;
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to get album pictures, Error: {0}", e.Message));
                return null;
            }
        }

        public IEnumerable<Album> GetEventAlbums(Event Event)
        {
            try
            {
                IEnumerable<Album> eventAlbums;

                lock (lockObj)
                {
                    eventAlbums = DBManager.Instance.RetrieveEventAlbums(Event);
                }
                log.Info("Get event albums: Successfully retrived");
                return eventAlbums;
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to get event albums, Error: {0}", e.Message));
                return null;
            }
        }

        public Tuple<bool,int,Picture> AddNewPhoto(Picture PictureToBeAdded)
        {
            try
            {
                Tuple<bool, int,Picture> result = new Tuple<bool, int, Picture>(false, 0, new Picture());

                bool IsPhotoExists = false;

                lock(lockObj)
                {
                    IsPhotoExists = DBManager.Instance.IsPhotoExists(PictureToBeAdded);
                }

                if (IsPhotoExists)
                {
                    log.Warn("Conflict: photo already exists");
                    return Tuple.Create(false, 0, new Picture() { PictureID = 0});
                }
                else
                {
                    lock (lockObj)
                    {
                        result = DBManager.Instance.AddPhoto(PictureToBeAdded);
                    }

                    log.Info("Photo: Successfully added");
                    return result;
                }   
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to add new photo, Error: {0}", e.Message));
                return Tuple.Create(false, 0, new Picture() { PictureID = 0});
            }
        }

        public bool AddPhotoToAlbum(Album SelectedAlbum, Picture SelectedPicture)
        {
            try
            {
                bool result = false;

                bool IsPhotoExistsInAlbum = false;
                bool IsPhotoExists = false;
                bool IsAlbumExists = false;

                lock(lockObj)
                {
                    IsAlbumExists = DBManager.Instance.IsAlbumExists(SelectedAlbum);
                }

                if (!IsAlbumExists)
                {
                    log.Warn("Conflict: album does not exists");
                    return false;
                }
                else
                {
                    lock(lockObj)
                    {
                        IsPhotoExists = DBManager.Instance.IsPhotoExists(SelectedPicture);
                    }

                    if (!IsPhotoExists)
                    {
                        log.Warn("Conflict: photo does not exists");
                        return false;
                    }
                    else
                    {
                        lock (lockObj)
                        {
                            IsPhotoExistsInAlbum = DBManager.Instance.IsPhotoExistsInAlbum(SelectedAlbum,SelectedPicture);
                        }

                        if (IsPhotoExistsInAlbum)
                        {
                            log.Warn("Conflict: photo already exists in album");
                            return false;
                        }
                        else
                        {
                            lock (lockObj)
                            {
                                result = DBManager.Instance.AddPhotoToAlbum(SelectedAlbum, SelectedPicture);
                            }

                            log.Info("Album: photo successfully added to album");
                            return result;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to add photo to album, Error: {0}", e.Message));
                return false;
            }
        }

        public bool DeletePhotoFromAlbum(Album SelectedAlbum, Picture SelectedPicture)
        {
            try
            {
                bool result = false;
                bool IsAlbumExists = false;
                bool IsPhotoExists = false;
                bool IsPhotoExistsInAlbum = false;

                lock(lockObj)
                {
                    IsAlbumExists = DBManager.Instance.IsAlbumExists(SelectedAlbum);
                }

                if (!IsAlbumExists)
                {
                    log.Warn("Conflict: album does not exists");
                    return false;
                }
                else
                {
                    lock(lockObj)
                    {
                        IsPhotoExists = DBManager.Instance.IsPhotoExists(SelectedPicture);
                    }
                    if (!IsPhotoExists)
                    {
                        log.Warn("Conflict: photo does not exists");
                        return false;
                    }
                    else
                    {
                        lock(lockObj)
                        {
                            IsPhotoExistsInAlbum = DBManager.Instance.IsPhotoExistsInAlbum(SelectedAlbum, SelectedPicture);
                        }

                        if (!IsPhotoExistsInAlbum)
                        {
                            log.Warn("Conflict: photo does not exists in album");
                            return false;
                        }
                        else
                        {
                            lock (lockObj)
                            {
                                result = DBManager.Instance.DeletePhotoFromAlbum(SelectedAlbum, SelectedPicture);
                            }
                            log.Info("Album: photo successfully deleted from album");
                            return result;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to delete photo from album, Error: {0}", e.Message));
                return false;
            }
        }

        public bool ModifyPhoto(Picture PictureToBeModified)
        {
            try
            {
                bool result = false;

                bool IsPhotoExist = false;

                lock(lockObj)
                {
                    IsPhotoExist = DBManager.Instance.IsPhotoExists(PictureToBeModified);
                }

                if (!IsPhotoExist)
                {
                    log.Warn("Conflict: photo does not exist");
                    return false;
                }
                else
                {
                    lock (lockObj)
                    {
                        result = DBManager.Instance.ModifyPhoto(PictureToBeModified);
                    }
                    log.Info("Photo: Successfully modified");
                    return result;
                }
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to modify photo, Error: {0}", e.Message));
                return false;
            }
        }

        public Picture DeletePhoto(Picture PictureToBeDeleted)
        {
            try
            {
                Picture result = null;
                bool IsPhotoExists = false;

                lock (lockObj)
                {
                    IsPhotoExists = DBManager.Instance.IsPhotoExists(PictureToBeDeleted);
                }

                if (!IsPhotoExists)
                {
                    log.Warn("Conflict: photo does not exists");
                    return null;
                }
                else
                {
                    lock (lockObj)
                    {
                        result = DBManager.Instance.DeletePhoto(PictureToBeDeleted);
                    }


                    return result;
                }
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to delete photo, Error: {0}", e.Message));
                return null;
            }
        }

        public Tuple<bool,int> AddNewAlbum(Album AlbumToBeAdded)
        {
            try
            {
                bool IsAlbumExists = false;
                Tuple<bool, int> result = new Tuple<bool, int>(false, 0);

                lock(lockObj)
                {
                    IsAlbumExists = DBManager.Instance.IsAlbumExists(AlbumToBeAdded);
                }

                if (IsAlbumExists)
                {
                    log.Warn("Conflict: album already exists");
                    return Tuple.Create(false, 0);
                }
                else
                {
                    lock (lockObj)
                    {
                        result = DBManager.Instance.AddAlbum(AlbumToBeAdded);
                    }
                    log.Info("Album: Successfully added");
                    return result;
                }
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to add album, Error: {0}", e.Message));
                return Tuple.Create(false, 0);
            }
        }

        public bool ModifyAlbum(Album AlbumToBeModified)
        {
            try
            {
                bool result = false;
                bool IsAlbumExists = false;

                lock(lockObj)
                {
                    IsAlbumExists = DBManager.Instance.IsAlbumExists(AlbumToBeModified);
                }

                if (!IsAlbumExists)
                {
                    log.Warn("Conflict: album does not exists");
                    return false;
                }
                else
                {
                    lock (lockObj)
                    {
                        result = DBManager.Instance.ModifyAlbum(AlbumToBeModified);
                    }
                    log.Info("Album: Successfully modified");
                    return result;
                }
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to modify album, Error: {0}", e.Message));
                return false;
            }
        }

        public bool DeleteAlbum(Album AlbumToBeDeleted)
        {
            try
            {
                bool result = false;
                bool IsAlbumExists = false;

                lock(lockObj)
                {
                    IsAlbumExists = DBManager.Instance.IsAlbumExists(AlbumToBeDeleted);
                }

                if (!IsAlbumExists)
                {
                    log.Warn("Conflict: album does not exists");
                    return false;
                }
                else
                {
                    lock (lockObj)
                    {
                        result = DBManager.Instance.DeleteAlbum(AlbumToBeDeleted);
                    }

                    log.Info("Album: Successfully deleted");
                    return result;
                }
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to delete album, Error: {0}", e.Message));
                return false;
            }
        }

        public Tuple<bool,int> AddNewEvent(Event EventToBeAdded)
        {
            try
            {
                Tuple<bool, int> result = new Tuple<bool, int>(false, 0);
                bool IsEventExists = false;

                lock(lockObj)
                {
                    IsEventExists = DBManager.Instance.IsEventExists(EventToBeAdded);
                }

                if (IsEventExists)
                {
                    log.Warn("Conflict: event already exists");
                    return Tuple.Create(false, 0);
                }
                else
                {
                    lock (lockObj)
                    {
                        result = DBManager.Instance.AddNewEvent(EventToBeAdded);
                    }
                    log.Info("Event: Successfully added");
                    return result;
                }
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to add new event, Error: {0}", e.Message));
                return Tuple.Create(false, 0);
            }
        }

        public bool ConnectAlbumToEvent(Album SelectedAlbum, Event SelectedEvent)
        {
            try
            {
                bool result = false;
                bool IsAlbumExists = false;
                bool IsEventExists = false;
                bool IsAlreadyConnected = false;

                lock(lockObj)
                {
                    IsEventExists = DBManager.Instance.IsEventExists(SelectedEvent);
                }

                if (!IsEventExists)
                {
                    log.Warn("Conflict: event does not exists");
                    return false;
                }
                else
                {
                    lock(lockObj)
                    {
                        IsAlbumExists = DBManager.Instance.IsAlbumExists(SelectedAlbum);
                    }

                    if (!IsAlbumExists)
                    {
                        log.Warn("Conflict: album does not exists");
                        return false;
                    }

                    else
                    {
                        lock(lockObj)
                        {
                            IsAlreadyConnected = DBManager.Instance.CheckIfConnected(SelectedEvent, SelectedAlbum);
                        }

                        if (IsAlreadyConnected)
                        {
                            log.Warn("Conflict: album is already connected to event");
                            return false;
                        }
                        else
                        {
                            lock (lockObj)
                            {
                                result = DBManager.Instance.ConnectAlbumToEvent(SelectedAlbum, SelectedEvent);
                            }
                            log.Info("Event: album successfully connected to event");
                            return result;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to connect album to event, Error: {0}", e.Message));
                return false;
            }
        }

        public bool RemoveAlbumFromEvent(Album SelectedAlbum, Event SelectedEvent)
        {
            try
            {
                bool result = false;
                bool IsEventExists = false;
                bool IsAlbumExists = false;
                bool IsAlreadyConnected = false;

                lock (lockObj)
                {
                    IsEventExists = DBManager.Instance.IsEventExists(SelectedEvent);
                }

                if (!IsEventExists)
                {
                    log.Warn("Conflict: event does not exists");
                    return false;
                }
                else
                {
                    lock (lockObj)
                    {
                        IsAlbumExists = DBManager.Instance.IsAlbumExists(SelectedAlbum);
                    }

                    if (!IsAlbumExists)
                    {
                        log.Warn("Conflict: album does not exists");
                        return false;
                    }

                    else
                    {
                        lock (lockObj)
                        {
                            IsAlreadyConnected = DBManager.Instance.CheckIfConnected(SelectedEvent, SelectedAlbum);
                        }

                        if (!IsAlreadyConnected)
                        {
                            log.Warn("Conflict: album is already removed from event");
                            return false;
                        }
                        else
                        {
                            lock (lockObj)
                            {
                                result = DBManager.Instance.RemoveAlbumFromEvent(SelectedAlbum, SelectedEvent);
                            }
                            log.Info("Event: album successfully removed from event");
                            return result;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(String.Format("Failed to remove album from event, Error: {0}", e.Message));
                return false;
            }
        }

        public bool ModifyEvent(Event EventToBeModified)
        {
            try
            {
                bool result = false;
                bool IsEventExists = false;

                lock(lockObj)
                {
                    IsEventExists = DBManager.Instance.IsEventExists(EventToBeModified);
                }

                if (!IsEventExists)
                {
                    log.Warn("Conflict: event does not exists");
                    return false;
                }
                else
                {
                    lock (lockObj)
                    {
                        result = DBManager.Instance.ModifyEvent(EventToBeModified);
                    }
                    log.Info("Event: Successfully modified");
                    return result;
                }
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to modify event, Error: {0}", e.Message));
                return false;
            }
        }

        public bool DeleteEvent(Event EventToBeDeleted)
        {
            try
            {
                bool result = false;
                bool IsEventExists = false;

                lock(lockObj)
                {
                    IsEventExists = DBManager.Instance.IsEventExists(EventToBeDeleted); 
                }

                if (!IsEventExists)
                {
                    log.Warn("Conflict: event does not exists");
                    return false;
                }
                else
                {
                    lock (lockObj)
                    {
                        result = DBManager.Instance.DeleteEvent(EventToBeDeleted);
                    }
                    log.Info("Event: Successfully deleted");
                    return result;
                }
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to delete event, Error: {0}", e.Message));
                return false;
            }
        }

        public double RatePhoto(Picture SelectedPicture, int rating, User user)
        {
            try
            {
                double result = 0;
                bool IsPhotoExists = false;
                User u = null;

                lock(lockObj)
                {
                    IsPhotoExists = DBManager.Instance.IsPhotoExists(SelectedPicture);
                }

                if(!IsPhotoExists)
                {
                    log.Warn("Conflict: photo does not exists");
                    return -1;
                }
                else
                {
                    lock(lockObj)
                    {
                        u = DBManager.Instance.GetUserByUsername(user.Username);
                    }

                    if (u == null)
                    {
                        log.Warn("Conflict: user does not exists");
                        return -1;
                    }
                    else
                    {
                        lock (lockObj)
                        {
                            result = DBManager.Instance.RatePhoto(SelectedPicture, rating, user);
                        }
                        log.Info("Photo: Successfully rated");
                        return result;
                    }
                }
            }
            catch(Exception e)
            {
                log.Error(String.Format("Failed to rate photo, Error: {0}", e.Message));
                return -1;
            }
        }
    }
}
