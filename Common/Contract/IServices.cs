using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Common.Model;

namespace Common.Contract
{
    [ServiceContract]
    public interface IServices
    {
        [OperationContract]
        bool ModifyUser(User modifiedUser, string usernameToModify);

        [OperationContract]
        bool AddNewUser(User UserToBeAdded);

        [OperationContract]
        IEnumerable<Picture> GetAllPhotos(User user);

        [OperationContract]
        IEnumerable<Album> GetAllAlbums(User user);

        [OperationContract]
        IEnumerable<Event> GetAllEvents();

        [OperationContract]
        IEnumerable<Picture> GetAlbumPictures(Album Album);

        [OperationContract]
        IEnumerable<Album> GetNonPrivateAlbums();

        [OperationContract]
        IEnumerable<Album> GetEventAlbums(Event Event);

        [OperationContract]
        Tuple<bool,int,Picture> AddNewPhoto(Picture PictureToBeAdded);

        [OperationContract]
        bool ModifyPhoto(Picture PictureToBeModified);

        [OperationContract]
        Picture DeletePhoto(Picture PictureToBeDeleted);

        [OperationContract]
        Tuple<bool,int> AddNewAlbum(Album AlbumToBeAdded);

        [OperationContract]
        bool ModifyAlbum(Album AlbumToBeModified);

        [OperationContract]
        bool DeleteAlbum(Album AlbumToBeDeleted);

        [OperationContract]
        Tuple<bool,int> AddNewEvent(Event EventToBeAdded);

        [OperationContract]
        bool ModifyEvent(Event EventToBeModified);

        [OperationContract]
        bool DeleteEvent(Event EventToBeDeleted);

        [OperationContract]
        bool AddPhotoToAlbum(Album SelectedAlbum, Picture SelectedPicture);

        [OperationContract]
        bool DeletePhotoFromAlbum(Album SelectedAlbum, Picture SelectedPicture);

        [OperationContract]
        bool ConnectAlbumToEvent(Album SelectedAlbum, Event SelectedEvent);

        [OperationContract]
        bool RemoveAlbumFromEvent(Album SelectedAlbum, Event SelectedEvent);

        [OperationContract]
        double RatePhoto(Picture SelectedPhoto, int rating, User u);
    }
}
