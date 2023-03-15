using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandInputReaderLibrary
{
    public abstract class ReadableGestureWithShortcuts : IReadableGesture
    {
        public int Priority { get; set; }

        protected List<IReadableGesture> possibleGestures;

        public ReadableGestureWithShortcuts()
        {
            possibleGestures = new List<IReadableGesture>();
        }

        public bool Read(List<ReadablePackage> inputs, int currentTime, Directions.FacingDirection facingDirection)
        {
            bool result = false;
            foreach (IReadableGesture gesture in possibleGestures)
            {
                if (gesture.Read(inputs, currentTime, facingDirection))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
