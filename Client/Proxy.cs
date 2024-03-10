using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Common.Contract;

namespace Client
{
    public class Proxy
    {
        public ILogin proxyServices;
        private static Proxy instance;

        private Proxy()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                MaxReceivedMessageSize = 20000000,
            };
            ChannelFactory<ILogin> factory = new ChannelFactory<ILogin>(binding, new EndpointAddress("net.tcp://localhost:8998/ILogin"));
            proxyServices = factory.CreateChannel();
            
        }

        //Singleton
        public static Proxy Instance()
        {
            if (instance == null)
            {
                instance = new Proxy();
            }
            return instance;
        }

    }
}
