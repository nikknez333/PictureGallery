using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Common.Model
{
    public class Album : INotifyPropertyChanged
    {
        private string name;
        private bool isPrivate;
        private DateTime date;
        private string createdByUser;

        public Album()
        {

        }

        public bool IsPrivate
        {
            get
            {
                return isPrivate;
            }
            set
            {
                if (isPrivate == value)
                    return;

                isPrivate = value;
                OnPropertyChanged(nameof(IsPrivate));
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


        public Event PartOfEvent { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AlbumID { get; set; }

        [IgnoreDataMember]
        public ICollection<Picture> Pictures { get; set; }

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
