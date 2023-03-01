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
}
