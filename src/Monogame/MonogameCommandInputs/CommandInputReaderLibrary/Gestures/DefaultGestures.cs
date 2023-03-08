using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandInputReaderLibrary;
using static CommandInputReaderLibrary.Directions;


namespace CommandInputReaderLibrary.Gestures
{
    public class QuarterCircleForward : ReadableGesture
    {
        public QuarterCircleForward()
        {
            Priority = 100; // arbitrary
        }

        protected override void ResetRequiredInputs()
        {
            base.ResetRequiredInputs();

            requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenSequentialInputs));
            requiredInputs.Push(new GestureComponent(Direction.DownForward, InputReader.TimeBetweenSequentialInputs));
            requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenSequentialInputs));

        }
    }
    public class QuarterCircleBack : ReadableGesture
    {
        public QuarterCircleBack()
        {
            Priority = 90; // arbitrary
        }

        protected override void ResetRequiredInputs()
        {
            base.ResetRequiredInputs();

            requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenSequentialInputs));
            requiredInputs.Push(new GestureComponent(Direction.DownBack, InputReader.TimeBetweenSequentialInputs));
            requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenSequentialInputs));

        }
    }

    public class HalfCircleForward : ReadableGestureWithShortcuts
    {
        private class Shortcut1 : ReadableGesture
        {
            protected override void ResetRequiredInputs()
            {
                base.ResetRequiredInputs();

                requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.DownForward, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenSequentialInputs));

            }
        }

        private class Shortcut2 : ReadableGesture
        {
            protected override void ResetRequiredInputs()
            {
                base.ResetRequiredInputs();

                requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.DownBack, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenNonSequentialInputs));

            }
        }

        public HalfCircleForward() : base()
        {
            possibleGestures.Add(new Shortcut1());
            possibleGestures.Add(new Shortcut2());
        }
    }
}
