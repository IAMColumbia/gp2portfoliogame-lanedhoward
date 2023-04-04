using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandInputReaderLibrary
{
    public class ReadPackage : IReadPackage
    {
        public float TimeReceived { get; set; }
        public IHostPackage mostRecentInputs { get; set; }
        public List<IReadableGesture> gestures { get; set; }
        public List<IButton> buttons { get; set; }
        public ReadPackage(IHostPackage _mostRecentInputs, List<IReadableGesture> _gestures, List<IButton> _buttons, float _TimeReceived)
        {
            mostRecentInputs = _mostRecentInputs;
            gestures = _gestures;
            buttons = _buttons;
            TimeReceived = _TimeReceived;
        }
    }

}