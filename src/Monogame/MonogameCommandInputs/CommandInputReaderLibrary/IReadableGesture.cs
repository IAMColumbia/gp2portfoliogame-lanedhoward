using CommandInputReaderLibrary;
using CommandInputReaderLibrary.Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandInputReaderLibrary
{
    public interface IReadableGesture : IGesture
    {
        /// <summary>
        /// Checks if the inputs meet the requirements of the gesture.
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public bool Read(List<ReadablePackage> inputs, int currentTime, Directions.FacingDirection facingDirection);
    }
}