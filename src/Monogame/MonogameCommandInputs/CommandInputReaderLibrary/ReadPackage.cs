using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandInputReaderLibrary
{
    public class ReadPackage : IReadPackage
    {
        public IHostPackage mostRecentInputs { get; set; }
        public List<IReadableGesture> gestures { get; set; }
        public List<IButton> buttons { get; set; }
        public ReadPackage(IHostPackage _mostRecentInputs, List<IReadableGesture> _gestures, List<IButton> _buttons)
        {
            mostRecentInputs = _mostRecentInputs;
            gestures = _gestures;
            buttons = _buttons;
        }
    }

}