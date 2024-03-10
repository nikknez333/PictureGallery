using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contract;
using Common.Model;

namespace Client.CommandPattern
{
    public class ServerReceiver : IServerReceiver
    {
        public override void AddNewPhoto(Picture photo, IServices proxy)
        {
            proxy.AddNewPhoto(photo);
        }

        public override void ModifyPhoto(Picture photo, IServices proxy)
        {
            proxy.ModifyPhoto(photo);
        }

        public override void UnModifyPhoto(Picture photo, IServices proxy)
        {
            proxy.ModifyPhoto(photo);
        }

        public override void DeletePhoto(Picture photo, IServices proxy)
        {
            proxy.DeletePhoto(photo);
        }

        public override void DuplicatePhoto(Picture photo, IServices proxy)
        {
            proxy.AddNewPhoto(photo);
        }
    }
}
