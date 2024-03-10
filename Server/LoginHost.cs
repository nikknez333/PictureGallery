using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Common.Contract;
using log4net;

namespace Server
{
    public class LoginHost
    {
        private ServiceHost loginServiceHost;
        private static readonly ILog log = LogHelper.GetLogger();

        public LoginHost()
        {
            loginServiceHost = new ServiceHost(typeof(LoginService));
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };

            loginServiceHost.AddServiceEndpoint(typeof(ILogin), binding, new Uri("net.tcp://localhost:8998/ILogin"));
        }

        public void Open()
        {
            try
            {
                loginServiceHost.Open();
                log.Info("LoginHost: Successfully started");
                Console.WriteLine(String.Format("LoginHost started successfully at {0}", DateTime.Now));
            }
            catch(Exception e)
            {
                log.Error(String.Format("LoginHost Open Error: {0}", e.Message));
                Console.WriteLine(String.Format("LoginHost open failed with error: {0}", e.Message));
            }
        }

        public void Close()
        {
            try
            {
                loginServiceHost.Close();
                log.Info("LoginHost: Successfully closed");
                Console.WriteLine(String.Format("LoginHost closed successfully at {0}", DateTime.Now));
            }
            catch(Exception e)
            {
                log.Error(String.Format("LoginHost Close Error: {0}", e.Message));
                Console.WriteLine(String.Format("LoginHost close failed with error: {0}", e.Message));
            }
        }
    }
}
