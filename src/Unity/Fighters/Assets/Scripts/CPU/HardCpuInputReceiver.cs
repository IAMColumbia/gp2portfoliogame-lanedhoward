using CommandInputReaderLibrary;
using LaneLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class HardCpuInputReceiver : FighterInputReceiver
{
    List<CpuCombo> combos;
    CpuCombo currentCombo;
    int comboIndex;
    bool lastHitConnected;

    Dictionary<int, int> upDownWeights;
    Dictionary<int, int> leftRightWeights;

    public HardCpuInputReceiver(FighterMain _fighter, FighterInputHost _inputHost, InputReader _inputReader) : base(_fighter, _inputHost, _inputReader)
    {
        fighter.AttackInRecovery += Fighter_AttackInRecovery;
        fighter.HitConnected += Fighter_HitConnected;

        upDownWeights = new Dictionary<int, int>();
        upDownWeights.Add(-1, 8);
        upDownWeights.Add(0, 10);
        upDownWeights.Add(1, 1);

        leftRightWeights = new Dictionary<int, int>();
        leftRightWeights.Add(-1, 5);
        leftRightWeights.Add(0, 1);
        leftRightWeights.Add(1, 7);

    }

    private void Fighter_HitConnected(object sender, EventArgs e)
    {
        lastHitConnected = true;
        comboIndex += 1;
    }

    private void Fighter_AttackInRecovery(object sender, EventArgs e)
    {
        if (currentCombo != null)
        {
            if (comboIndex >= currentCombo.moves.Count)
            {
                // finished the combo
                currentCombo = null;
                return;
            }

            if (currentCombo.moves[comboIndex].needsToConnect && !lastHitConnected)
            {
                //dropped the combo
                currentCombo = null;
            }
            else
            {
                comboIndex += 1;
            }
        }
    }

    public override void SetPossibleGestures(List<IReadableGesture> possibleGestures)
    {
        combos = fighter.characterModule.GetCpuCombos();
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
                if (currentCombo == null)
                {
                    currentCombo = RandomMethods.Choose(combos.Where(c => c.CanExecute(fighter)).ToList());

                    if (currentCombo != null)
                    {
                        // found a combo we can execute
                        // start it at 0
                        comboIndex = 0;
                    }
                }

                if (currentCombo != null)
                {
                    if (comboIndex >= currentCombo.moves.Count)
                    {
                        // finished the combo
                        currentCombo = null;
                    }
                    else
                    {
                        // do the current move in the combo
                        List<IReadableGesture> gestures = new List<IReadableGesture>();
                        List<IButton> buttons = new List<IButton>();

                        gestures.Add(currentCombo.moves[comboIndex].gesture);
                        buttons.Add(currentCombo.moves[comboIndex].button);

                        IReadPackage package = new ReadPackage(null, gestures, buttons, 0);
                
                        bufferedInput = package;
                        bufferedAttackTime = 0;
                        lastHitConnected = false;

                    }

                }

            }

            ChooseMoveDirections();
        }
        return false;
    }

    public void ChooseMoveDirections()
    {
        UpDown = RandomMethods.ChooseWeighted(upDownWeights);
        LeftRight = RandomMethods.ChooseWeighted(leftRightWeights);
        if (fighter.facingDirection != Directions.FacingDirection.RIGHT)
        {
            LeftRight *= -1;
        }
    }

    public override void UpdateFacingDirection()
    {
        // do nothing
    }
}