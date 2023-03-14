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
        private int Time; // time is an int, measured in ticks / frames

        HostPackageComparer comparer;

        // STATIC MEMBERS
        public static int TimeBetweenSequentialInputs = 8;
        public static int TimeBetweenNonSequentialInputs = 16;



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

        private PriorityQueue<IReadableGesture, int> ReadAllGestures()
        {
            PriorityQueue<IReadableGesture, int> foundGestures = new PriorityQueue<IReadableGesture, int>();

            foreach (IReadableGesture gesture in readableGestures)
            {
                if (gesture.Read(inputs, Time, facingDirection))
                {
                    foundGestures.Enqueue(gesture, gesture.Priority);
                }
            }

            return foundGestures;
        }

        public IReadPackage? Tick()
        {
            // TECHNICIAL DEBT: null object pattern
            Time++;

            IReadPackage? resultReadPackage = null;

            bool foundNewInput = GetCurrentInputs();

            if (foundNewInput)
            {
                PriorityQueue<IReadableGesture, int> foundGestures = ReadAllGestures();

                IHostPackage lastInput = inputs.Last();

                PriorityQueue<IButton, int> buttons = new PriorityQueue<IButton, int>();

                if (lastInput.Buttons.Count > 0)
                {
                    foreach (var b in lastInput.Buttons)
                    {
                        buttons.Enqueue(b, b.Priority);
                    }
                }

                resultReadPackage = new ReadPackage(lastInput, foundGestures, buttons);
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