using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandInputReaderLibrary
{
    public abstract class Button : IButton
    {
        public int Priority { get; set; }

        public IButton.ButtonState State { get; set; }

    }
}
