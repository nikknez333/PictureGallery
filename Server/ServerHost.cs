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
    public class ServerHost
    {
        private ServiceHost serviceHost;
        private static readonly ILog log = LogHelper.GetLogger();

        public ServerHost()
        {
            serviceHost = new ServiceHost(typeof(Services));
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
                MaxReceivedMessageSize = 20000000,
            };

            serviceHost.AddServiceEndpoint(typeof(IServices), binding, new Uri("net.tcp://localhost:8999/IServices"));
        }

        public void Open()
        {
            try
            {
                serviceHost.Open();
                log.Info("ServerHost: Successfully started");
                Console.WriteLine(String.Format("Server successfully started at {0}", DateTime.Now));
            }
            catch(Exception e)
            {
                log.Error(String.Format("ServerHost Open Error: {0}", e.Message));
                Console.WriteLine(String.Format("Open failed with error: {0}", e.Message));
            }
        }

        public void Close()
        {
            try
            {
                serviceHost.Close();
                log.Info("ServerHost: Successfully closed");
                Console.WriteLine(String.Format("Server Services successfully closed at {0}", DateTime.Now));
            }
            catch(Exception e)
            {
                log.Error(String.Format("ServerHost Close Error: {0}", e.Message));
                Console.WriteLine(String.Format("Close failed with error: {0}", e.Message));
            }
        }
    }
}
