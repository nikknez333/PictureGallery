using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.LastIndexOf("bin")) + "Database";
            AppDomain.CurrentDomain.SetData("DataDirectory", path);

            ServerHost serverServices = new ServerHost();
            serverServices.Open();

            LoginHost loginService = new LoginHost();
            loginService.Open();

            Console.ReadLine();
            loginService.Close();
            serverServices.Close();
        }
    }
}
