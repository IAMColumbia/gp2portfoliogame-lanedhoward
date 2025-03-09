using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandInputReaderLibrary;

public class GameMoveInput
{
    public IGesture gesture;

    public IButton button;

    public GameMoveInput(IGesture g, IButton b)
    {
        gesture = g;
        button = b;
    }


    public override bool Equals(Object obj)
    {
        if (!(obj is GameMoveInput)) return false;

        GameMoveInput p = (GameMoveInput)obj;
        return (gesture.GetType() == p.gesture.GetType()) & (button.GetType() == p.button.GetType());
    }

    public override int GetHashCode()
    {
        return gesture.GetType().GetHashCode() * button.GetType().GetHashCode();
    }
}
