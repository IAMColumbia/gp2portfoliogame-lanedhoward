using CommandInputReaderLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandInputReaderLibrary
{
    
    public class InputReader
    {
        public IInputHost inputHost;
        public List<ReadablePackage> inputs;

        public List<IReadableGesture> readableGestures; // all possible gestures

        private PriorityQueue<IReadableGesture, int> gesturesQueue; // this is gonna get reset a lot

        public bool FacingRight;
        public int Time; // time is an int, measured in frames

        public static int TimeBetweenSequentialInputs = 4;
        public static int TimeBetweenNonSequentialInputs = 8;

        public InputReader(IInputHost host)
        {
            inputHost = host;
            inputs = new List<ReadablePackage>();
            readableGestures = new List<IReadableGesture>();
            gesturesQueue = new PriorityQueue<IReadableGesture, int>();
            FacingRight = true;
        }

        private void ResetGesturesPriorityQueue()
        {
            gesturesQueue = new PriorityQueue<IReadableGesture, int>();

            foreach (IReadableGesture gesture in readableGestures)
            {
                gesturesQueue.Enqueue(gesture, gesture.Priority);
            }
        }

        private void GetCurrentInputs()
        {
            IHostPackage hostPackage = inputHost.GetCurrentInputs();

            ReadablePackage readablePackage = new ReadablePackage(hostPackage, Time);

            inputs.Add(readablePackage);
        }


    }
}