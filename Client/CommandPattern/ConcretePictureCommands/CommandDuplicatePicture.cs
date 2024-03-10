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
    public class CommandDuplicatePicture : PictureCommand
    {
        public CommandDuplicatePicture(Picture photo, ServerReceiver receiver, IServices proxy, ObservableCollection<Picture> Pictures) : base(photo, receiver, proxy, Pictures)
        {
                
        }

        public override void Execute()
        {
            this.serverReceiver.AddNewPhoto(this.photo, this.proxy);
            Pictures.Add(this.photo);
        }

        public override void UnExecute()
        {
            this.serverReceiver.DeletePhoto(this.photo, this.proxy);
            Pictures.Remove(this.photo);
        }
    }
}
