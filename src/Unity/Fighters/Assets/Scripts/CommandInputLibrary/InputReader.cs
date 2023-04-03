using CommandInputReaderLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandInputReaderLibrary
{
    
    public class InputReader
    {
        // PRIVATE MEMBERS
        private IInputHost inputHost;
        private List<ReadablePackage> inputs;

        private List<IReadableGesture> readableGestures; // all possible gestures

        private Directions.FacingDirection facingDirection;
        private float Time; // time is an float, measured in ticks / frames

        private float AssumedTicksPerSecond = 60; 

        HostPackageComparer comparer;

        // STATIC MEMBERS
        public static float TimeBetweenSequentialInputs = 8;
        public static float TimeBetweenNonSequentialInputs = 16;

        public static float MinChargeTime = 35;
        public static float MaxTimeBetweenChargePartitions = 16;
        public static float MaxTimeBetweenChargeAndRelease = 16;
        public static float MaxTimeAfterRelease = 8;


        public InputReader(IInputHost host)
        {
            inputHost = host;
            inputs = new List<ReadablePackage>();
            readableGestures = new List<IReadableGesture>();
            facingDirection = Directions.FacingDirection.RIGHT;
            Time = 0;
            comparer = new HostPackageComparer();
        }

        /// <summary>
        /// Checks for current inputs from the InputHost. If there are new inputs, adds them to the inputs list.
        /// Returns true if got a new input, false if no input change
        /// </summary>
        private bool GetCurrentInputs()
        {
            IHostPackage hostPackage = inputHost.GetCurrentInputs();

            if (inputs.Count > 0)
            {
                if (comparer.Equals(hostPackage, inputs.Last())) 
                {
                    // inputs haven't changed since last time
                    return false;
                }
            }
            ReadablePackage readablePackage = new ReadablePackage(hostPackage, Time);

            
            inputs.Add(readablePackage);

            return true;
        }

        private List<IReadableGesture> ReadAllGestures()
        {
            List<IReadableGesture> foundGestures = new List<IReadableGesture>();

            foreach (IReadableGesture gesture in readableGestures)
            {
                if (gesture.Read(inputs, Time, facingDirection))
                {
                    foundGestures.Add(gesture);
                }
            }

            return foundGestures.OrderBy(g => g.Priority).ToList();
        }

        public IReadPackage? Tick(TimeSpan elapsedTime)
        {
            // TECHNICIAL DEBT: null object pattern
            Time += (float)elapsedTime.TotalSeconds * AssumedTicksPerSecond;

            IReadPackage? resultReadPackage = null;

            bool foundNewInput = GetCurrentInputs();

            if (foundNewInput)
            {
                List<IReadableGesture> foundGestures = ReadAllGestures();

                IHostPackage lastInput = inputs.Last();

                List<IButton> buttons = new List<IButton>();

                if (lastInput.Buttons.Count > 0)
                {
                    foreach (var b in lastInput.Buttons)
                    {
                        buttons.Add(b);
                    }
                }

                buttons = buttons.OrderBy(b => b.Priority).ToList();

                resultReadPackage = new ReadPackage(lastInput, foundGestures, buttons, Time);
            }

            return resultReadPackage;
        }

        public void SetPossibleGestures(List<IReadableGesture> gestures)
        {
            readableGestures = gestures;
        }

        public void ChangeFacingDirection()
        {
            if (facingDirection == Directions.FacingDirection.LEFT)
            {
                ChangeFacingDirection(Directions.FacingDirection.RIGHT);
                return;
            }
            ChangeFacingDirection(Directions.FacingDirection.LEFT);
        }
        public void ChangeFacingDirection(Directions.FacingDirection newDirection)
        {
            facingDirection = newDirection;
        }

        public Directions.FacingDirection GetFacingDirection()
        {
            return facingDirection;
        }

        public void SetInputHost(IInputHost host)
        {
            inputHost = host;
        }

        public IInputHost GetInputHost()
        {
            return inputHost;
        }
    }
}