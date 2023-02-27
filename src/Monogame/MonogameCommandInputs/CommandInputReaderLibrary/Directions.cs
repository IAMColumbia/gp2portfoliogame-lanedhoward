using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameCommandInputs
{
    public static class Directions
    {
        public enum Direction
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
    }
}
