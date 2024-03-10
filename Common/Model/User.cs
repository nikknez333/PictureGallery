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
    public class User : INotifyPropertyChanged
    {
        private string username;
        private string password;
        private string name;
        private string surname;
        private TypeOfUser type;

        [IgnoreDataMember]
        public List<int> ratedPictures { get; set; }

        public User()
        {
            ratedPictures = new List<int>();
        }

        public User(string username)
        {
            Username = username;
        }
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                if (username == value)
                    return;

                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        [Required]
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (password == value)
                    return;

                password = value;
                OnPropertyChanged(nameof(Password));
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

        public string Surname
        {
            get
            {
                return surname;
            }
            set
            {
                if (surname == value)
                    return;

                surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        public TypeOfUser Type
        {
            get
            {
                return type;
            }
            set
            {
                if (type == value)
                    return;

                type = value;
                OnPropertyChanged(nameof(Type));
            }
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
