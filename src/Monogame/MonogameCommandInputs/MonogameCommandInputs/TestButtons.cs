using CommandInputReaderLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameCommandInputs
{
    public class Punch : Button
    {
        public Punch() : base()
        {
            Priority = 50;
        }
    }

    public class Kick : Button
    {
        public Kick() : base()
        {
            Priority = 40;
        }
    }
}
