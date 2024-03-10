using Common.Contract;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.CommandPattern
{
    public abstract class PictureCommand : Command
    {
        protected IServices proxy;
        protected IServerReceiver serverReceiver;
        protected Picture photo;
        protected ObservableCollection<Picture> Pictures { get; set; }

        public PictureCommand(Picture photo, ServerReceiver serverReceiver, IServices proxy, ObservableCollection<Picture> Pictures)
        {
            this.photo = photo;
            this.serverReceiver = serverReceiver;
            this.proxy = proxy;
            this.Pictures = Pictures;
        }
    }
}
