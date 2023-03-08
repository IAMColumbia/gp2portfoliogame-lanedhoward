using CommandInputReaderLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandInputReaderLibrary
{
    
    public class InputReader
    {
        // PUBLIC MEMBERS
        public IInputHost inputHost;
        public List<ReadablePackage> inputs;

        public List<IReadableGesture> readableGestures; // all possible gestures

        public Directions.FacingDirection facingDirection;
        public int Time; // time is an int, measured in ticks / frames

        // PRIVATE MEMBERS

        // STATIC MEMBERS
        public static int TimeBetweenSequentialInputs = 4;
        public static int TimeBetweenNonSequentialInputs = 8;



        public InputReader(IInputHost host)
        {
            inputHost = host;
            inputs = new List<ReadablePackage>();
            readableGestures = new List<IReadableGesture>();
            facingDirection = Directions.FacingDirection.RIGHT;
        }

        /// <summary>
        /// Checks for current inputs from the InputHost. If there are new inputs, adds them to the inputs list.
        /// Returns true if got a new input, false if no input change
        /// </summary>
        private bool GetCurrentInputs()
        {
            IHostPackage hostPackage = inputHost.GetCurrentInputs();

            if (hostPackage == inputs.Last()) 
            {
                // inputs haven't changed since last time
                return false;
            }

            ReadablePackage readablePackage = new ReadablePackage(hostPackage, Time);

            // TODO: shouldn't add new hostpackage if it was the same as the previous.
            // like if youre holding the buttons down, we dont need a new hostpackage every frame
            // until you release one
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

            IReadPackage? resultReadPackage = null;

            bool foundNewInput = GetCurrentInputs();

            if (foundNewInput)
            {
                PriorityQueue<IReadableGesture, int> foundGestures = ReadAllGestures();

                resultReadPackage = new ReadPackage(inputs.Last(), foundGestures);
            }

            return resultReadPackage;
        }
    }
}