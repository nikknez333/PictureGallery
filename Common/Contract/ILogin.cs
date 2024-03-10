using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Common.Model;

namespace Common.Contract
{
    [ServiceContract]
    public interface ILogin
    {
        [OperationContract]
        User Login(string username, string password);
    }
}
