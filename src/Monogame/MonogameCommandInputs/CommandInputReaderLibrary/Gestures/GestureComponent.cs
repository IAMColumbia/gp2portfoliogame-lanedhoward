using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandInputReaderLibrary.Gestures
{
    public class GestureComponent
    {
        public Directions.Direction Direction;
        public int MaxTimeSinceLastInput;

        public GestureComponent(Directions.Direction direction, int maxTimeSinceLastInput)
        {
            Direction = direction;
            MaxTimeSinceLastInput = maxTimeSinceLastInput;
        }
    }
}
