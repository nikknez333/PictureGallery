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
    public class CommandModifyPicture : PictureCommand
    {
        private Picture UnModifiedPicture { get; set; }

        public CommandModifyPicture(Picture photo, ServerReceiver receiver, IServices proxy, ObservableCollection<Picture>Pictures, Picture UnModifiedPicture) : base(photo, receiver, proxy, Pictures)
        {
            this.UnModifiedPicture = UnModifiedPicture;
        }

        public override void Execute()
        {
            UnModifiedPicture = photo;
            this.serverReceiver.ModifyPhoto(this.photo, this.proxy);
        }

        public override void UnExecute()
        {
            this.serverReceiver.UnModifyPhoto(this.UnModifiedPicture, this.proxy);
        }
    }
}
