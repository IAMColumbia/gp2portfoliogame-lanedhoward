using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandInputReaderLibrary
{
    public class ReadablePackage : HostPackage
    {
        public int TimeReceived;

        public ReadablePackage(IHostPackage hostPackage, int timeReceived) //, bool facingRight)
        {
            this.UpDown = hostPackage.UpDown;
            this.LeftRight = hostPackage.LeftRight;

            this.Buttons = hostPackage.Buttons;

            this.TimeReceived = timeReceived;
            //this.FacingRight = facingRight;
        }

        public Directions.Direction GetDirectionFacingForward(Directions.FacingDirection facingDirection)
        {
            return Directions.GetDirectionFacingForward(UpDown, LeftRight, facingDirection);
        }
    }
}