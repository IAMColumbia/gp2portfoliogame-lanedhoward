using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandInputReader
{
    public class HostPackage : IHostPackage
    {
        public int LeftRight { get; set; }
        public int UpDown { get; set; }
    }
}