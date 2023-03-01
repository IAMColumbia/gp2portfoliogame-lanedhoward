using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandInputReaderLibrary
{
    public class ReadablePackage : HostPackage
    {
        public int TimeReceived;
        //public bool FacingRight; // not sure if this will be necessary

        public ReadablePackage(IHostPackage hostPackage, int timeReceived) //, bool facingRight)
        {
            this.UpDown = hostPackage.UpDown;
            this.LeftRight = hostPackage.LeftRight;

            this.TimeReceived = timeReceived;
            //this.FacingRight = facingRight;
        }

        public Directions.Direction GetDirectionFacingRight()
        {
            return Directions.GetDirectionFacingRight(UpDown, LeftRight);
        }
    }
}