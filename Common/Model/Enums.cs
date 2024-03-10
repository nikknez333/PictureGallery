using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public enum TypeOfUser : int
    {
        USER,
        ADMIN
    }

    public enum EventType : int
    {
        DEBUG,
        INFO,
        WARN,
        ERROR,
        FATAL
    }
}
