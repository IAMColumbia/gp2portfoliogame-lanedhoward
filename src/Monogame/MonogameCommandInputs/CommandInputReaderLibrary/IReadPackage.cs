using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandInputReaderLibrary
{
    public interface IReadPackage
    {
        IHostPackage mostRecentInputs { get; set; }

        PriorityQueue<IReadableGesture, int> gestures { get; set; }

        PriorityQueue<IButton, int> buttons { get; set; }
    }
}