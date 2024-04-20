using CommandInputReaderLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class TrainingInputReceiver : FighterInputReceiver
{
    public enum BlockSetting
    {
        Nothing,
        Everything,
        AfterFirstHit,
        OnlyFirstHit,
        Random
    }

    public BlockSetting blockSetting;

    public TrainingInputReceiver(FighterMain _fighter, FighterInputHost _inputHost, InputReader _inputReader) : base(_fighter, _inputHost, _inputReader)
    {
        fighter.LeftHitstun += Fighter_LeftHitstun;
        fighter.LeftBlockstun += Fighter_LeftBlockstun;
        fighter.GotHit += Fighter_GotHit;
        fighter.Blocked += Fighter_Blocked;
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

    public void ChangeBlockSetting()
    {
        switch (blockSetting)
        {
            case BlockSetting.Nothing:
                blockSetting = BlockSetting.Everything;
                fighter.blockEverything = true;
                fighter.SendNotification("Block Mode: Everything");
                break;
                // afterfirsthit and onlyfirsthit dont work rn so just always go to random
            case BlockSetting.Everything:
                //blockSetting = BlockSetting.AfterFirstHit;
                //fighter.blockEverything = false;
                //fighter.SendNotification("Block Mode: AfterFirstHit");
                //break;
            case BlockSetting.AfterFirstHit:
                //blockSetting = BlockSetting.OnlyFirstHit;
                //fighter.blockEverything = true;
                //fighter.SendNotification("Block Mode: OnlyFirstHit");
                //break;
            case BlockSetting.OnlyFirstHit:
                blockSetting = BlockSetting.Random;
                fighter.blockEverything = LaneLibrary.RandomMethods.RANDOM.Next(2) == 0;
                fighter.SendNotification("Block Mode: Random");
                break;
            case BlockSetting.Random:
                blockSetting = BlockSetting.Nothing;
                fighter.blockEverything = false;
                fighter.SendNotification("Block Mode: Nothing");
                break;
        }

    }
    private void Fighter_Blocked(object sender, EventArgs e)
    {
        switch (blockSetting)
        {
            default:
                return;
            case BlockSetting.AfterFirstHit:
            case BlockSetting.OnlyFirstHit:
                fighter.blockEverything = false;
                return;
            case BlockSetting.Random:
                fighter.blockEverything = LaneLibrary.RandomMethods.RANDOM.Next(2) == 0;
                return;
        }
    }

    private void Fighter_GotHit(object sender, EventArgs e)
    {
        switch (blockSetting)
        {
            default:
                return;
            case BlockSetting.AfterFirstHit:
            case BlockSetting.OnlyFirstHit:
                fighter.blockEverything = true;
                return;
            case BlockSetting.Random:
                fighter.blockEverything = LaneLibrary.RandomMethods.RANDOM.Next(2) == 0;
                return;
        }
    }

    private void Fighter_LeftHitstun(object sender, EventArgs e)
    {
        switch (blockSetting)
        {
            default:
                return;
            case BlockSetting.AfterFirstHit:
            case BlockSetting.OnlyFirstHit:
                fighter.blockEverything = true;
                return;
            case BlockSetting.Random:
                fighter.blockEverything = LaneLibrary.RandomMethods.RANDOM.Next(2) == 0;
                return;
        }
    }

    private void Fighter_LeftBlockstun(object sender, EventArgs e)
    {
        switch (blockSetting)
        {
            default:
                return;
            case BlockSetting.AfterFirstHit:
                fighter.blockEverything = false;
                return;
            case BlockSetting.OnlyFirstHit:
                fighter.blockEverything = false;
                return;
            case BlockSetting.Random:
                fighter.blockEverything = LaneLibrary.RandomMethods.RANDOM.Next(2) == 0;
                return;
        }
    }
}