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
        /// Make sure your inputs are facing right!
        /// </summary>
        /// <param name="inputsFacingRight"></param>
        /// <returns></returns>
        public bool Read(List<ReadablePackage> inputsFacingRight, float currentTime);
    }
}