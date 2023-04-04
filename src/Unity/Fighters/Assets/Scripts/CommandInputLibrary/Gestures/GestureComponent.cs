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
        public float MaxTimeSinceLastInput;

        public GestureComponent(Directions.Direction direction, float maxTimeSinceLastInput)
        {
            Direction = direction;
            MaxTimeSinceLastInput = maxTimeSinceLastInput;
        }
    }
}
