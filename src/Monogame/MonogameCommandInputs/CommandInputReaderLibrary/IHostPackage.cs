using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonogameCommandInputs
{

    public interface IHostPackage
    {
        public int LeftRight { get; set; }
        public int UpDown { get; set; }
    }
}