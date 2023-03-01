using CommandInputReaderLibrary.Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandInputReaderLibrary
{
    public abstract class ReadableGesture : IReadableGesture
    {
        public int Priority { get; set; }
        protected Stack<GestureComponent> requiredInputs { get; set; }

        public ReadableGesture()
        {
            requiredInputs = new Stack<GestureComponent>();
        }

        public virtual bool Read(List<ReadablePackage> inputsFacingRight, float currentTime)
        {
            ResetRequiredInputs();

            GestureComponent gestureComponent;

            if (!requiredInputs.TryPop(out gestureComponent))
            {
                // no gesture components ????
                throw new Exception("There were no gesture components in the gesture at all");
                // later this should probably just return true? so there could be an empty gesture? 
            }

            for (int i = inputsFacingRight.Count-1; i > 0; i--) // start at most recent input and go backwards
            {
                ReadablePackage package = inputsFacingRight[i];

                if (currentTime - package.TimeReceived > gestureComponent.MaxTimeSinceLastInput)
                {
                    // its been too long since the last input
                    return false;
                }

                if (package.GetDirectionFacingRight() == gestureComponent.Direction)
                {
                    // this is the right input, and its within the time frame
                    currentTime = package.TimeReceived; // new current time from the last successful input

                    if (!requiredInputs.TryPop(out gestureComponent))
                    {
                        // no gesture components left , we mustve completed the gesture
                        return true;
                    }

                }
            }

            // if we made it through the whole loop without returning, assume we didnt complete the gesture
            return false;
        }

        protected virtual void ResetRequiredInputs()
        {
            requiredInputs = new Stack<GestureComponent>();
        }
    }
}
