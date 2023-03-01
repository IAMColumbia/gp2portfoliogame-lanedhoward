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
            ReadableGesture qcf = new QuarterCircleForward();

            List<ReadablePackage> inputs = new List<ReadablePackage>();

            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = 0 }, 0));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = -1 }, 1));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = -1 }, 2));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = 0 }, 3));

            //act
            bool foundQcf = qcf.Read(inputs, 4);

            //assert
            Assert.IsTrue(foundQcf);
        }

        [TestMethod]
        public void ExtraInputQcf()
        {
            //arrange
            ReadableGesture qcf = new QuarterCircleForward();

            List<ReadablePackage> inputs = new List<ReadablePackage>();

            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = 0 }, 0));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = -1 }, 1));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = 0 }, 2));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = -1 }, 3));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = 0 }, 4));

            //act
            bool foundQcf = qcf.Read(inputs, 5);

            //assert
            Assert.IsTrue(foundQcf);
        }

        [TestMethod]
        public void SlowQcf()
        {
            //arrange
            ReadableGesture qcf = new QuarterCircleForward();

            List<ReadablePackage> inputs = new List<ReadablePackage>();

            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = 0 }, 0));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 0, UpDown = -1 }, 5));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = -1 }, 10));
            inputs.Add(new ReadablePackage(new HostPackage() { LeftRight = 1, UpDown = 0 }, 15));

            //act
            bool foundQcf = qcf.Read(inputs, 16);

            //assert
            Assert.IsFalse(foundQcf);
        }
    }
}