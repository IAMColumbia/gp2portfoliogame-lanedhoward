using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandInputReaderLibrary;
using static CommandInputReaderLibrary.Directions;


namespace CommandInputReaderLibrary.Gestures
{
    public static class DefaultGestures
    {
        public static List<IReadableGesture> GetDefaultGestures()
        {
            List<IReadableGesture> gestures = new List<IReadableGesture>()
            {
                new QuarterCircleBack(),
                new QuarterCircleForward(),
                new DragonPunch(),
                new HalfCircleBack(),
                new HalfCircleForward(),
                new ThreeSixty(),
                new DoubleQuarterCircleBack(),
                new DoubleQuarterCircleForward(),
                new BackForwardCharge(),
                new DownUpCharge()
            };
            return gestures;
        }
    }

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
            Priority = 70;

            possibleGestures.Add(new Shortcut1());
            possibleGestures.Add(new Shortcut2());
        }
    }

    public class HalfCircleBack : ReadableGestureWithShortcuts
    {
        private class Shortcut1 : ReadableGesture
        {
            protected override void ResetRequiredInputs()
            {
                base.ResetRequiredInputs();

                requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.DownBack, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenSequentialInputs));

            }
        }

        private class Shortcut2 : ReadableGesture
        {
            protected override void ResetRequiredInputs()
            {
                base.ResetRequiredInputs();

                requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.DownForward, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenNonSequentialInputs));

            }
        }

        public HalfCircleBack() : base()
        {
            Priority = 60;

            possibleGestures.Add(new Shortcut1());
            possibleGestures.Add(new Shortcut2());
        }
    }

    public class ThreeSixty : ReadableGestureWithShortcuts
    {
        private class Shortcut1 : ReadableGesture
        {
            protected override void ResetRequiredInputs()
            {
                base.ResetRequiredInputs();

                requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Up, InputReader.TimeBetweenNonSequentialInputs));

            }
        }

        private class Shortcut2 : ReadableGesture
        {
            protected override void ResetRequiredInputs()
            {
                base.ResetRequiredInputs();

                requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Up, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenNonSequentialInputs));

            }
        }

        private class Shortcut3 : ReadableGesture
        {
            protected override void ResetRequiredInputs()
            {
                base.ResetRequiredInputs();

                requiredInputs.Push(new GestureComponent(Direction.Up, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenNonSequentialInputs)); ;

            }
        }
        private class Shortcut4 : ReadableGesture
        {
            protected override void ResetRequiredInputs()
            {
                base.ResetRequiredInputs();

                requiredInputs.Push(new GestureComponent(Direction.Up, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenNonSequentialInputs)); ;

            }
        }

        private class Shortcut5 : ReadableGesture
        {
            protected override void ResetRequiredInputs()
            {
                base.ResetRequiredInputs();

                requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Up, InputReader.TimeBetweenNonSequentialInputs)); ;

            }
        }

        private class Shortcut6 : ReadableGesture
        {
            protected override void ResetRequiredInputs()
            {
                base.ResetRequiredInputs();

                requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Up, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenNonSequentialInputs)); ;

            }
        }
        private class Shortcut7 : ReadableGesture
        {
            protected override void ResetRequiredInputs()
            {
                base.ResetRequiredInputs();

                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Up, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenNonSequentialInputs)); ;

            }
        }
        private class Shortcut8 : ReadableGesture
        {
            protected override void ResetRequiredInputs()
            {
                base.ResetRequiredInputs();

                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Up, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenNonSequentialInputs)); ;

            }
        }

        public ThreeSixty() : base()
        {
            Priority = 10;

            possibleGestures.Add(new Shortcut1());
            possibleGestures.Add(new Shortcut2());
            possibleGestures.Add(new Shortcut3());
            possibleGestures.Add(new Shortcut4());
            possibleGestures.Add(new Shortcut5());
            possibleGestures.Add(new Shortcut6());
            possibleGestures.Add(new Shortcut7());
            possibleGestures.Add(new Shortcut8());
        }
    }

    public class DragonPunch : ReadableGesture
    {
        public DragonPunch()
        {
            Priority = 50; // arbitrary
        }

        protected override void ResetRequiredInputs()
        {
            base.ResetRequiredInputs();

            requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenSequentialInputs));
            requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenNonSequentialInputs));
            requiredInputs.Push(new GestureComponent(Direction.DownForward, InputReader.TimeBetweenSequentialInputs));

            disallowedInputs.Add(new GestureComponent(Direction.Back, 0));
            disallowedInputs.Add(new GestureComponent(Direction.UpBack, 0));
            disallowedInputs.Add(new GestureComponent(Direction.DownBack, 0));

        }
    }

    public class DoubleQuarterCircleForward : ReadableGestureWithShortcuts
    {
        private class Shortcut1 : ReadableGesture
        {
            protected override void ResetRequiredInputs()
            {
                base.ResetRequiredInputs();

                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.DownForward, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenNonSequentialInputs));

            }
        }

        private class Shortcut2 : ReadableGesture
        {
            protected override void ResetRequiredInputs()
            {
                base.ResetRequiredInputs();

                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.DownForward, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenSequentialInputs));

            }
        }

        public DoubleQuarterCircleForward() : base()
        {
            Priority = 40;

            possibleGestures.Add(new Shortcut1());
            possibleGestures.Add(new Shortcut2());
        }
    }

    public class DoubleQuarterCircleBack : ReadableGestureWithShortcuts
    {
        private class Shortcut1 : ReadableGesture
        {
            protected override void ResetRequiredInputs()
            {
                base.ResetRequiredInputs();

                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.DownBack, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenNonSequentialInputs));

            }
        }

        private class Shortcut2 : ReadableGesture
        {
            protected override void ResetRequiredInputs()
            {
                base.ResetRequiredInputs();

                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenNonSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.DownBack, InputReader.TimeBetweenSequentialInputs));
                requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenSequentialInputs));

            }
        }

        public DoubleQuarterCircleBack() : base()
        {
            Priority = 40;

            possibleGestures.Add(new Shortcut1());
            possibleGestures.Add(new Shortcut2());
        }
    }

    public class BackForwardCharge : ReadableChargeGesture
    {
        public BackForwardCharge() : base()
        {
            Priority = 130;

            minChargeTime = InputReader.MinChargeTime;
            maxTimeBetweenChargePartitions = InputReader.MaxTimeBetweenChargePartitions;
            maxTimeBetweenChargeAndRelease = InputReader.MaxTimeBetweenChargeAndRelease;
            maxTimeAfterRelease = InputReader.MaxTimeAfterRelease;

            chargeDirections.Add(Direction.Back);
            chargeDirections.Add(Direction.UpBack);
            chargeDirections.Add(Direction.DownBack);

            releaseDirections.Add(Direction.Forward);
            releaseDirections.Add(Direction.DownForward);
        }
    }

    public class DownUpCharge : ReadableChargeGesture
    {
        public DownUpCharge() : base()
        {
            Priority = 120;

            minChargeTime = InputReader.MinChargeTime;
            maxTimeBetweenChargePartitions = InputReader.MaxTimeBetweenChargePartitions;
            maxTimeBetweenChargeAndRelease = InputReader.MaxTimeBetweenChargeAndRelease;
            maxTimeAfterRelease = InputReader.MaxTimeAfterRelease;

            chargeDirections.Add(Direction.Down);
            chargeDirections.Add(Direction.DownForward);
            chargeDirections.Add(Direction.DownBack);

            releaseDirections.Add(Direction.Up);
            releaseDirections.Add(Direction.UpForward);
            releaseDirections.Add(Direction.UpBack);
        }
    }
}
