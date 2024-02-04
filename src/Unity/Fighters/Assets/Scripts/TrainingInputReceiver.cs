using CommandInputReaderLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TrainingInputReceiver : FighterInputReceiver
{
    public TrainingInputReceiver(FighterMain _fighter, FighterInputHost _inputHost, InputReader _inputReader) : base(_fighter, _inputHost, _inputReader)
    {
    }

    public override void SetPossibleGestures(List<IReadableGesture> possibleGestures)
    {
        // do nothing
    }

    public override bool CheckForInputs()
    {
        return false;
    }

    public override void UpdateFacingDirection()
    {
        // do nothing
    }
}