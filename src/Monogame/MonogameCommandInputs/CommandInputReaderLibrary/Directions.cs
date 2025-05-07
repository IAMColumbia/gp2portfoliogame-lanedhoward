using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandInputReaderLibrary
{
    public static class Directions
    {
        public enum Direction : byte
        {
            Up,
            UpForward,
            Forward,
            DownForward,
            Down,
            DownBack,
            Back,
            UpBack,
            Neutral
        }

        public enum FacingDirection : byte
        {
            RIGHT,
            LEFT
        }

        public static Direction FlipDirection(Direction d)
        {
            Direction r;
            switch (d)
            {
                case Direction.UpForward:
                    r = Direction.UpBack;
                    break;
                case Direction.Forward:
                    r = Direction.Back;
                    break;
                case Direction.DownForward:
                    r = Direction.DownBack;
                    break;
                case Direction.UpBack:
                    r = Direction.UpForward;
                    break;
                case Direction.Back:
                    r = Direction.Forward;
                    break;
                case Direction.DownBack:
                    r = Direction.DownForward;
                    break;
                default:
                    r = d;
                    break;
            }
            return r;
        }

        public static Direction GetDirectionFacingForward(int upDown, int leftRight, FacingDirection facingDirection)
        {
            Direction r = Direction.Neutral;
            switch (upDown)
            {
                case 1:
                    switch (leftRight)
                    {
                        case 1:
                            r = Direction.UpForward;
                            break;
                        case 0:
                            r = Direction.Up;
                            break;
                        case -1:
                            r = Direction.UpBack;
                            break;
                    }
                    break;
                case 0:
                    switch (leftRight)
                    {
                        case 1:
                            r = Direction.Forward;
                            break;
                        case 0:
                            r = Direction.Neutral;
                            break;
                        case -1:
                            r = Direction.Back;
                            break;
                    }
                    break;
                case -1:
                    switch (leftRight)
                    {
                        case 1:
                            r = Direction.DownForward;
                            break;
                        case 0:
                            r = Direction.Down;
                            break;
                        case -1:
                            r = Direction.DownBack;
                            break;
                    }
                    break;
            }

            if (facingDirection == Directions.FacingDirection.LEFT)
            {
                r = Directions.FlipDirection(r);
            }
            return r;
        }
        
    }
}
