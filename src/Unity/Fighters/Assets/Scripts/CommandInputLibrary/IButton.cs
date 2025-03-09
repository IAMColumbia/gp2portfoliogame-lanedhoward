using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandInputReaderLibrary
{
    public interface IButton
    {
        public enum ButtonState
        {
            Pressed,
            Held,
            Released
        }
        public int Priority { get; set; }

        public ButtonState State { get; set; }
    }
}
