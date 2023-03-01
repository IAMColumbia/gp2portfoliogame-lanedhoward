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
        public int MaxTimeToExecute { get; set; }
        protected Stack<GestureComponent> requiredInputs { get; set; }

        public ReadableGesture()
        {
            requiredInputs = new Stack<GestureComponent>();
        }

        public virtual bool Read(List<ReadablePackage> inputsFacingRight, float currentTime)
        {
            throw new NotImplementedException();
        }

        protected virtual void ResetRequiredInputs()
        {
            requiredInputs = new Stack<GestureComponent>();
        }
    }
}
