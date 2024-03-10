using Common.Contract;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.CommandPattern
{
    public abstract class IServerReceiver
    {
        public abstract void AddNewPhoto(Picture photo, IServices proxy);
        public abstract void ModifyPhoto(Picture photo, IServices proxy);
        public abstract void UnModifyPhoto(Picture photo, IServices proxy);
        public abstract void DeletePhoto(Picture photo, IServices proxy);
        public abstract void DuplicatePhoto(Picture photo, IServices proxy);
    }
}
