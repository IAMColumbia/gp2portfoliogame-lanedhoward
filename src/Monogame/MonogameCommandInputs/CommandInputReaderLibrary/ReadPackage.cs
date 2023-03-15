using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandInputReaderLibrary
{
    public class ReadPackage : IReadPackage
    {
        public IHostPackage mostRecentInputs { get; set; }
        public PriorityQueue<IReadableGesture, int> gestures { get; set; }
        public PriorityQueue<IButton, int> buttons { get; set; }
        public ReadPackage(IHostPackage _mostRecentInputs, PriorityQueue<IReadableGesture, int> _gestures, PriorityQueue<IButton, int> _buttons)
        {
            mostRecentInputs = _mostRecentInputs;
            gestures = _gestures;
            buttons = _buttons;
        }
    }

}