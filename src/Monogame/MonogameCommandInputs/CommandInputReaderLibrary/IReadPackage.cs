using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandInputReaderLibrary
{
    public interface IReadPackage
    {
        public float TimeReceived { get; set; }
        IHostPackage mostRecentInputs { get; set; }

        List<IReadableGesture> gestures { get; set; }

        List<IButton> buttons { get; set; }
    }
}