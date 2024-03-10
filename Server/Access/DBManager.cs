using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Common.Model;

namespace Server.Access
{
    class DBManager
    {
        private static DBManager instance;

        private DBManager()
        {

        }

        public static DBManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DBManager();
                }

                return instance;
            }
        }

        public bool UserExist(string username)
        {           
            using (var dbContext = new PhotoGalleryContext())
            {
                bool exist = false;

                if (dbContext.Users.Any(u => u.Username == username))
                    exist = true;

                return exist;
            }           
        }

        public bool AddUser(User u)
        {
            using(var dbContext = new PhotoGalleryContext())
            {
                bool IsInDB = dbContext.Users.Any(usr => usr.Username == u.Username);

                if (IsInDB)
                    return false;

                User user = dbContext.Users.Add(u);
               
                dbContext.SaveChanges();
                
                return true;
            }
        }

        public User GetUserByUsername(string username)
        {
            using(var dbContext = new PhotoGalleryContext())
            {
                return dbContext.Users.Find(username);
            }
        }

        public bool ModifyUser(User u, string usernameToModify)
        {
            using(var dbContext = new PhotoGalleryContext())
            {
                User userToModify = dbContext.Users.FirstOrDefault(e => e.Username == usernameToModify);
                if(userToModify == null)
                {
                    return false;
                }

                userToModify.Name = u.Name;
                userToModify.Surname = u.Surname;

                dbContext.SaveChanges();

                return true;
            }
        }

        public IEnumerable<Picture> RetrieveAllPhotos(User u)
        {
            IEnumerable<Picture> photos;       
            IQueryable<Picture> nonAlbumPhotos;
            IQueryable<Picture> albumPhotos;

            using (var dbContext = new PhotoGalleryContext())
            {
                nonAlbumPhotos = dbContext.Pictures.Where(p => p.PartOfAlbum == null);

                //sve slike koje su deo nekog albuma(sve koje nisu deo privatnog albuma, a ako su deo privatnog albuma samo od korisnika koji ih je kreirao
                albumPhotos = dbContext.Pictures.Where(p => p.PartOfAlbum != null && p.PartOfAlbum.IsPrivate == false || (p.PartOfAlbum.IsPrivate == true && p.PartOfAlbum.CreatedByUser == u.Username));

                photos = (nonAlbumPhotos.Union(albumPhotos)).ToList();

                return photos;
            }
        }

        public IEnumerable<Picture> RetrieveAlbumPictures(Album a)
        {
            using (var dbContext = new PhotoGalleryContext())
            {
                IEnumerable<Picture> albumPictures;

                Album album = dbContext.Albums.Include(alb => alb.Pictures).FirstOrDefault(alb => alb.AlbumID == a.AlbumID);

                if (album == null || album.Pictures == null)
                    return null;

                albumPictures = (album.Pictures).ToList();

                return albumPictures;
            }
        }

        public IEnumerable<Album> RetrieveEventAlbums(Event e)
        {
            IEnumerable<Album> eventAlbums;

            using (var dbContext = new PhotoGalleryContext())
            {
                Event evnt = dbContext.Events.Include(et => et.Albums).FirstOrDefault(evt => evt.EventID == e.EventID);

                if (evnt == null || evnt.Albums == null)
                    return null;

                eventAlbums = (evnt.Albums).ToList();

                return eventAlbums;
            }
        }

        public IEnumerable<Album> RetrieveAllAlbums(User u)
        {
            IEnumerable<Album> albums;

            using (var dbContext = new PhotoGalleryContext())
            {

                albums = dbContext.Albums.ToList();

                return albums;
            }
        }

        public IEnumerable<Album> RetrieveAllNonPrivateAlbums()
        {       
            using (var dbContext = new PhotoGalleryContext())
            {
                IEnumerable<Album> albums;
                IQueryable<Album> nonPrivateAlbums;
              
                nonPrivateAlbums = dbContext.Albums.Include(alb => alb.Pictures).Where(a => a.IsPrivate == false);
                albums = nonPrivateAlbums.ToList();

                return albums;
            }
        }

        public IEnumerable<Event> RetrieveAllEvents()
        {
            IEnumerable<Event> events;

            using (var dbContext = new PhotoGalleryContext())
            {
                events = dbContext.Events.ToList();

                return events;
            }
        }

        public Tuple<bool,int,Picture> AddPhoto(Picture p)
        {
            using(var dbContext = new PhotoGalleryContext())
            {
                bool IsInDB = dbContext.Pictures.Any(pict => pict.PictureID == p.PictureID);

                if (IsInDB)
                    return new Tuple<bool, int, Picture>(false, 0, new Picture() { PictureID = 0});

                Picture pic = dbContext.Pictures.Add(p);
                dbContext.SaveChanges();

                return Tuple.Create(true, pic.PictureID, pic);
            }
        }

        public bool IsPhotoExists(Picture p)
        {
            using (var dbContext = new PhotoGalleryContext())
            {
                return dbContext.Pictures.Any(pic => pic.PictureID == p.PictureID);
            }
        }

        public bool IsAlbumExists(Album a)
        {
            using(var dbContext = new PhotoGalleryContext())
            {
                return dbContext.Albums.Any(alb => alb.AlbumID == a.AlbumID);
            }
        }

        public bool IsPhotoExistsInAlbum(Album a, Picture p)
        {
            using(var dbContext = new PhotoGalleryContext())
            {
                return dbContext.Albums.Any(alb => alb.Pictures.Any(pict => pict.PictureID == p.PictureID));
            }
        }

        public bool IsEventExists(Event e)
        {
            using (var dbContext = new PhotoGalleryContext())
            {
                return dbContext.Events.Any(evt => evt.EventID == e.EventID);
            }
        }

        public bool CheckIfConnected(Event e, Album a)
        {
            using(var dbContext = new PhotoGalleryContext())
            {
                return dbContext.Events.Any(evt => evt.Albums.Any(alb => alb.AlbumID == a.AlbumID));
            }
        }

        public bool AddPhotoToAlbum(Album a, Picture p)
        {
            using (var dbContext = new PhotoGalleryContext())
            {
                bool IsThereAlbum = dbContext.Albums.Any(alb => alb.AlbumID == a.AlbumID);
                bool IsTherePicture = dbContext.Pictures.Any(pic => pic.PictureID == p.PictureID);

                if (!IsThereAlbum && !IsTherePicture)
                    return false;

                Picture pict = dbContext.Pictures.FirstOrDefault(pic => pic.PictureID == p.PictureID);
                Album album = dbContext.Albums.Include(al=>al.Pictures).FirstOrDefault(alb => alb.AlbumID == a.AlbumID);

                if (pict.PartOfAlbum == album)
                    return false;
                if (album.Pictures.Contains(pict))
                    return false;

                pict.PartOfAlbum = album;
                album.Pictures.Add(pict);
                dbContext.SaveChanges();

                return true;
            }
        }

        public bool ModifyPhoto(Picture p)
        {
            using (var dbContext = new PhotoGalleryContext())
            {
                Picture pict = dbContext.Pictures.FirstOrDefault(pic => pic.PictureID == p.PictureID);

                if(pict == null)
                {
                    return false;
                }

                pict.Name = p.Name;
                pict.Date = p.Date;
                pict.Tags = p.Tags;
                pict.Image = p.Image;

                dbContext.SaveChanges();

                return true;
            }
        }

        public Picture DeletePhoto(Picture p)
        {
            using(var dbContext = new PhotoGalleryContext())
            {
                Picture pict = dbContext.Pictures.FirstOrDefault(pic => pic.PictureID == p.PictureID);

                if (pict == null)
                    return null;

                Picture removedPicture = dbContext.Pictures.Remove(pict);

                dbContext.SaveChanges();

                return removedPicture;
            }
        }

        public bool DeletePhotoFromAlbum(Album a, Picture p)
        {
            using (var dbContext = new PhotoGalleryContext())
            {
                Album album = dbContext.Albums.Include(al=>al.Pictures).FirstOrDefault(alb => alb.AlbumID == a.AlbumID);
                Picture pict = dbContext.Pictures.FirstOrDefault(pic => pic.PictureID == p.PictureID);

                if (album == null || pict == null)
                    return false;

                pict.PartOfAlbum = null;
                album.Pictures.Remove(pict);

                dbContext.SaveChanges();

                return true;
            }
        }

        public Tuple<bool,int> AddAlbum(Album a)
        {
            using (var dbContext = new PhotoGalleryContext())
            {
                bool IsInDB = dbContext.Albums.Any(alb => alb.AlbumID == a.AlbumID);

                if (IsInDB)
                    return Tuple.Create(false, 0);

                Album album = dbContext.Albums.Add(a);

                dbContext.SaveChanges();

                return Tuple.Create(true, album.AlbumID);
            }
        }

        public bool ConnectAlbumToEvent(Album a, Event e)
        {
            using (var dbContext = new PhotoGalleryContext())
            {
                bool IsThereAlbum = dbContext.Albums.Any(alb => alb.AlbumID == a.AlbumID);
                bool IsThereEvent = dbContext.Events.Any(evt => evt.EventID == e.EventID);

                if (!IsThereAlbum && !IsThereEvent)
                    return false;

                Event evnt = dbContext.Events.Include(et=>et.Albums).FirstOrDefault(evt => evt.EventID == e.EventID);
                Album album = dbContext.Albums.FirstOrDefault(alb => alb.AlbumID == a.AlbumID);


                album.PartOfEvent = evnt;
                evnt.Albums.Add(album);

                dbContext.SaveChanges();

                return true;
            }
        }

        public bool ModifyAlbum(Album a)
        {
            using(var dbContext = new PhotoGalleryContext())
            {
                Album album = dbContext.Albums.FirstOrDefault(alb => alb.AlbumID == a.AlbumID);

                if (album == null)
                    return false;

                album.Name = a.Name;
                album.Date = a.Date;
                album.IsPrivate = a.IsPrivate;

                dbContext.SaveChanges();

                return true;
            }
        }

        public bool DeleteAlbum(Album a)
        {
            using(var dbContext = new PhotoGalleryContext())
            {
                Album album = dbContext.Albums.FirstOrDefault(alb => alb.AlbumID == a.AlbumID);

                if (album == null)
                    return false;

                dbContext.Albums.Remove(album);

                dbContext.SaveChanges();

                return true;
            }
        }

        public bool RemoveAlbumFromEvent(Album a, Event e)
        {
            using (var dbContext = new PhotoGalleryContext())
            {
                Album album = dbContext.Albums.FirstOrDefault(alb => alb.AlbumID == a.AlbumID);
                Event evnt = dbContext.Events.Include(et=>et.Albums).FirstOrDefault(evt => evt.EventID == e.EventID);

                if (album == null || evnt == null)
                    return false;

                album.PartOfEvent = null;
                evnt.Albums.Remove(album);

                dbContext.SaveChanges();

                return true;
            }
        }

        public Tuple<bool,int> AddNewEvent(Event e)
        {
            using (var dbContext = new PhotoGalleryContext())
            {
                bool IsInDB = dbContext.Events.Any(evt => evt.EventID == e.EventID);

                if (IsInDB)
                    return Tuple.Create(false, 0);

                Event evnt = dbContext.Events.Add(e);

                dbContext.SaveChanges();

                return Tuple.Create(true, evnt.EventID);
            }
        }

        public bool ModifyEvent(Event e)
        {
            using (var dbContext = new PhotoGalleryContext())
            {
                Event evnt = dbContext.Events.FirstOrDefault(evt => evt.EventID == e.EventID);

                if(evnt == null)
                   return false;

                evnt.Name = e.Name;
                evnt.Date = e.Date;
                evnt.Location = e.Location;

                dbContext.SaveChanges();

                return true;
            }
        }

        public bool DeleteEvent(Event e)
        {
            using(var dbContext = new PhotoGalleryContext())
            {
                Event evnt = dbContext.Events.FirstOrDefault(evt => evt.EventID == e.EventID);

                if (evnt == null)
                    return false;

                dbContext.Events.Remove(evnt);

                dbContext.SaveChanges();

                return true;
            }
        }

        public double RatePhoto(Picture SelectedPicture, int rating, User u)
        {
            using(var dbContext = new PhotoGalleryContext())
            {
                Picture photo = dbContext.Pictures.FirstOrDefault(pic => pic.PictureID == SelectedPicture.PictureID);
                User user = dbContext.Users.FirstOrDefault(usr => usr.Username == u.Username);

                if (photo == null)
                    return -1;

                user.ratedPictures.Add(photo.PictureID);
                photo.TimesRated++;
                photo.RatingSum += rating;
                photo.Rating = photo.RatingSum / photo.TimesRated;

                dbContext.SaveChanges();

                return photo.Rating;
            }
        }
    }
}
