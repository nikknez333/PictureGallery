using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Picture : INotifyPropertyChanged, ICloneable
    {
        private string name;
        private DateTime date;
        private string tags;
        private double rating;
        private byte[] image;
        private string createdByUser;

        [IgnoreDataMember]
        public double TimesRated { get; set; }

        [IgnoreDataMember]
        public double RatingSum { get; set; }

        public Picture()
        {
            PictureID = 0;
            TimesRated = 0;
            RatingSum = 0;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PictureID { get; set; }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name == value)
                    return;

                name = value;

                OnPropertyChanged(nameof(Name));
            }
        }

        public byte[] Image
        {
            get
            {
                return image;
            }
            set
            {
                if (image == value)
                    return;

                image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                if (date == value)
                    return;

                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }
        public string Tags
        {
            get
            {
                return tags;
            }
            set
            {
                if (tags == value)
                    return;

                tags = value;
                OnPropertyChanged(nameof(Tags));
            }
        }
        public double Rating
        {
            get
            {
                return rating;
            }
            set
            {
                if (rating == value)
                    return;

                rating = value;
                OnPropertyChanged(nameof(Rating));
            }
        }

        public string CreatedByUser
        {
            get
            {
                return createdByUser;
            }
            set
            {
                if (createdByUser == value)
                    return;

                createdByUser = value;
                OnPropertyChanged(nameof(CreatedByUser));
            }
        }

        public Album PartOfAlbum { get; set; }
  

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public object Clone()
        {
            Picture pictureCopy = this.MemberwiseClone() as Picture;
            pictureCopy.PictureID = this.GetHashCode();

            if (this.PartOfAlbum != null)
            {
                pictureCopy.PartOfAlbum = new Album();
                pictureCopy.PartOfAlbum.Name = this.PartOfAlbum.Name;
                pictureCopy.PartOfAlbum.IsPrivate = this.PartOfAlbum.IsPrivate;
                pictureCopy.PartOfAlbum.CreatedByUser = this.PartOfAlbum.CreatedByUser;
                pictureCopy.PartOfAlbum.AlbumID = this.PartOfAlbum.AlbumID;
            
                if (PartOfAlbum.Pictures.Count != 0)
                {
                    foreach (Picture p in PartOfAlbum.Pictures)
                    {
                        pictureCopy.PartOfAlbum.Pictures.Add(p);
                    }
                }
            }

            return pictureCopy;
        }

    }
}
