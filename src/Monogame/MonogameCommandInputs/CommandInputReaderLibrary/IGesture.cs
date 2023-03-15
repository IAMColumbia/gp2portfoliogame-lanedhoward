using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandInputReaderLibrary
{
    public interface IGesture
    {
        // need some way to keep track of type maybe? an enum?
        public int Priority { get; set; }
    }
}