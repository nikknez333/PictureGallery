using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contract;
using Common.Model;
using log4net;
using Server.Access;

namespace Server
{
    public class LoginService : ILogin
    {
        private static readonly Object lockObj = new object();
        private static readonly ILog log = LogHelper.GetLogger();

        public User Login(string username, string password)
        {
            User loginUser = null;
            try
            {
                lock (lockObj)
                {
                    loginUser = DBManager.Instance.GetUserByUsername(username);
                }

                if (loginUser == null)
                {
                    log.Info("Login: Invalid Username");
                    return null;
                }
                if (loginUser.Password != password)
                {
                    log.Info("Login: Invalid Password");
                }
            }
            catch(Exception e)
            {
                log.Error(String.Format("Login Server Error: {0}", e.Message));
                return loginUser;
            }

            log.Info("Login: Successfully executed");
            return loginUser;
        }
    }
}
