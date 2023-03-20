using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandInputReaderLibrary
{

    public interface IHostPackage
    {
        public int LeftRight { get; set; }
        public int UpDown { get; set; }

        public List<IButton> Buttons { get; set; }
    }

    
}