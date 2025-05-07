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

        protected List<GestureComponent> disallowedInputs { get; set; }

        public ReadableGesture()
        {
            requiredInputs = new Stack<GestureComponent>();
            disallowedInputs = new List<GestureComponent>();
        }

        public virtual bool Read(List<ReadablePackage> inputs, float currentTime, Directions.FacingDirection facingDirection)
        {
            ResetRequiredInputs();

            GestureComponent gestureComponent;

            if (!requiredInputs.TryPop(out gestureComponent))
            {
                // no gesture components ????
                throw new Exception("There were no gesture components in the gesture at all");
                // later this should probably just return true? so there could be an empty gesture? 
            }

            for (int i = inputs.Count - 1; i > 0; i--) // start at most recent input and go backwards
            {
                ReadablePackage package = inputs[i];

                // we do timing like this so that:
                // 1. we can still read gestures that started with holding their first button (ex. holding back -> half circle forward)
                // 2. we don't read gestures more than once if you hold the last button for a long time and then release
                bool thisIsTheLastInputWeShouldCheck = (currentTime - package.TimeReceived > gestureComponent.MaxTimeSinceLastInput);

                Directions.Direction packageDirection = package.GetDirectionFacingForward(facingDirection);

                if (packageDirection == gestureComponent.Direction)
                {

                    if (!thisIsTheLastInputWeShouldCheck)
                    {
                        currentTime = package.TimeReceived; // new current time from the last successful input
                    }

                    if (!requiredInputs.TryPop(out gestureComponent))
                    {
                        // no gesture components left , we mustve completed the gesture
                        return true;
                    }

                }

                if (thisIsTheLastInputWeShouldCheck)
                {
                    // its been too long since the last input
                    return false;
                }

                if (disallowedInputs.Any(g => g.Direction == packageDirection))
                {
                    // has a disallowed input
                    return false;
                }
            }

            // if we made it through the whole loop without returning, assume we didnt complete the gesture
            return false;
        }

        protected virtual void ResetRequiredInputs()
        {
            requiredInputs = new Stack<GestureComponent>();
            disallowedInputs = new List<GestureComponent>();
        }
    }
}
