using Common.Contract;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.CommandPattern.ConcretePictureCommands
{
    public class CommandRemovePicture : PictureCommand
    {
        public CommandRemovePicture(Picture photo, ServerReceiver receiver, IServices proxy, ObservableCollection<Picture> Pictures) : base(photo, receiver, proxy, Pictures)
        {

        }

        public override void Execute()
        {
            this.serverReceiver.DeletePhoto(this.photo, this.proxy);
            Pictures.Remove(this.photo);
        }

        public override void UnExecute()
        {
            this.serverReceiver.AddNewPhoto(this.photo, this.proxy);
            Pictures.Add(this.photo);
        }
    }
}
