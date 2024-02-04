using CommandInputReaderLibrary;
using LaneLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class CpuInputReceiver : FighterInputReceiver
{
    List<IReadableGesture> possibleGestures;
    List<IButton> possibleButtons;
    Dictionary<int, int> upDownWeights;
    Dictionary<int, int> leftRightWeights;

    public CpuInputReceiver(FighterMain _fighter, FighterInputHost _inputHost, InputReader _inputReader) : base(_fighter, _inputHost, _inputReader)
    {
        possibleButtons = new List<IButton>()
        {
            new AttackA(),
            new AttackB(),
            new AttackC(),
            new AttackD(),
            new DashMacro()
        };

        upDownWeights = new Dictionary<int, int>();
        upDownWeights.Add(-1, 9);
        upDownWeights.Add(0, 10);
        upDownWeights.Add(1, 1);

        leftRightWeights = new Dictionary<int, int>();
        leftRightWeights.Add(-1, 4);
        leftRightWeights.Add(0, 1);
        leftRightWeights.Add(1, 5);

    }

    public override void SetPossibleGestures(List<IReadableGesture> possibleGestures)
    {
        this.possibleGestures = possibleGestures;
    }

    // happens every Update
    // basically our Update()
    public override bool CheckForInputs()
    {
        ManageBuffer();

        if (RandomMethods.RANDOM.Next(18) == 0)
        {
            if (RandomMethods.RANDOM.Next(4) == 0)
            {
                List<IReadableGesture> gestures = new List<IReadableGesture>();
                List<IButton> buttons = new List<IButton>();

                gestures.Add(RandomMethods.Choose(possibleGestures));
                if (RandomMethods.RANDOM.Next(2) != 0) gestures.Add(new NoGesture());
                buttons.Add(RandomMethods.Choose(possibleButtons));

                IReadPackage package = new ReadPackage(null, gestures, buttons, 0);
                
                bufferedInput = package;
                bufferedAttackTime = 0;
            }

            UpDown = RandomMethods.ChooseWeighted(upDownWeights);
            LeftRight = RandomMethods.ChooseWeighted(leftRightWeights);
            if (fighter.facingDirection != Directions.FacingDirection.RIGHT)
            {
                LeftRight *= -1;
            }

            //fighter.blockEverything = RandomMethods.RANDOM.Next(2) == 0;
        }
        return false;
    }

    public override void UpdateFacingDirection()
    {
        // do nothing
    }
}