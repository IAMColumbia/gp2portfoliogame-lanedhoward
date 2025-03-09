using CommandInputReaderLibrary;
using CommandInputReaderLibrary.Gestures;

namespace CommandInputTests
{
    [TestClass]
    public class GesturesTest
    {
        [TestMethod]
        public void PerfectQcf()
        {
            //arrange
            IReadableGesture qcf = new QuarterCircleForward();

            List<ReadablePackage> inputs = new List<ReadablePackage>();

            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = 0 }, 0));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = -1 }, 1));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = -1 }, 2));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = 0 }, 3));

            //act
            bool foundQcf = qcf.Read(inputs, 4, Directions.FacingDirection.RIGHT);

            //assert
            Assert.IsTrue(foundQcf);
        }

        [TestMethod]
        public void ExtraInputQcf()
        {
            //arrange
            IReadableGesture qcf = new QuarterCircleForward();

            List<ReadablePackage> inputs = new List<ReadablePackage>();

            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = 0 }, 0));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = -1 }, 1));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = 0 }, 2));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = -1 }, 3));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = 0 }, 4));

            //act
            bool foundQcf = qcf.Read(inputs, 5, Directions.FacingDirection.RIGHT);

            //assert
            Assert.IsTrue(foundQcf);
        }

        [TestMethod]
        public void SlowQcf()
        {
            //arrange
            IReadableGesture qcf = new QuarterCircleForward();

            List<ReadablePackage> inputs = new List<ReadablePackage>();

            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = 0 }, 0));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = -1 }, 9));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = -1 }, 18));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = 0 }, 27));

            //act
            bool foundQcf = qcf.Read(inputs, 28, Directions.FacingDirection.RIGHT);

            //assert
            Assert.IsFalse(foundQcf);
        }

        [TestMethod]
        public void LeftQcf()
        {
            //arrange
            IReadableGesture qcf = new QuarterCircleForward();

            List<ReadablePackage> inputs = new List<ReadablePackage>();

            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = 0 }, 0));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = -1 }, 1));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = -1, UpDown = -1 }, 2));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = -1, UpDown = 0 }, 3));

            //act
            bool foundQcf = qcf.Read(inputs, 4, Directions.FacingDirection.LEFT);

            //assert
            Assert.IsTrue(foundQcf);
        }

        [TestMethod]
        public void PerfecHcf()
        {
            //arrange
            IReadableGesture hcf = new HalfCircleForward();

            List<ReadablePackage> inputs = new List<ReadablePackage>();

            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = 0 }, 0));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = -1, UpDown = 0 }, 1));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = -1, UpDown = -1 }, 2));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = -1 }, 3));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = -1 }, 4));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = 0 }, 5));

            //act
            bool foundQcf = hcf.Read(inputs, 6, Directions.FacingDirection.RIGHT);

            //assert
            Assert.IsTrue(foundQcf);
        }

        [TestMethod]
        public void Shortcut1Hcf()
        {
            //arrange
            IReadableGesture hcf = new HalfCircleForward();

            List<ReadablePackage> inputs = new List<ReadablePackage>();

            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = 0 }, 0));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = -1, UpDown = 0 }, 1));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = -1 }, 3));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = -1 }, 4));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = 0 }, 5));

            //act
            bool foundQcf = hcf.Read(inputs, 6, Directions.FacingDirection.RIGHT);

            //assert
            Assert.IsTrue(foundQcf);
        }

        [TestMethod]
        public void Shortcut2Hcf()
        {
            //arrange
            IReadableGesture hcf = new HalfCircleForward();

            List<ReadablePackage> inputs = new List<ReadablePackage>();

            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = 0 }, 0));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = -1, UpDown = 0 }, 1));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = -1, UpDown = -1 }, 2));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = -1 }, 3));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = 0 }, 5));

            //act
            bool foundQcf = hcf.Read(inputs, 6, Directions.FacingDirection.RIGHT);

            //assert
            Assert.IsTrue(foundQcf);
        }
    }
}