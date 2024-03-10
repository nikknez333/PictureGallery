using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.CommandPattern
{
    public interface IInvoker
    {
        List<Command> CommandsList { get; set; }
        Int32 currentCommand { get; set; }

        bool Undo();
        bool Redo();

        void RememberCommand(Command commandToAdd);
    }
}
