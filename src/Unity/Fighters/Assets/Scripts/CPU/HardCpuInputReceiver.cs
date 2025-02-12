﻿using CommandInputReaderLibrary;
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
    int timesPressedAtComboIndex;
    Dictionary<int, int> upDownWeightsClose;
    Dictionary<int, int> upDownWeightsFar;
    Dictionary<int, int> leftRightWeightsClose;
    Dictionary<int, int> leftRightWeightsFar;

    int mashRateClose = 3;
    int mashRateFar = 7;

    bool isClose;
    float closeDistance = 3f;

    float timer;
    float timerMax = 1f / 60f;

    public HardCpuInputReceiver(FighterMain _fighter, FighterInputHost _inputHost, InputReader _inputReader) : base(_fighter, _inputHost, _inputReader)
    {
        fighter.AttackInRecovery += Fighter_AttackInRecovery;
        fighter.AttackActive += Fighter_AttackActive;
        fighter.HitConnected += Fighter_HitConnected;
        fighter.GotHit += Fighter_GotHit;
        fighter.Blocked += StopCombo;
        fighter.ThrowTeched += StopCombo;
        fighter.Parried += Fighter_Parried;

        upDownWeightsFar = new Dictionary<int, int>
        {
            { -1, 5 },
            { 0, 15 },
            { 1, 1 }
        };

        leftRightWeightsFar = new Dictionary<int, int>
        {
            { -1, 5 },
            { 0, 2 },
            { 1, 15 }
        };

        upDownWeightsClose = new Dictionary<int, int>
        {
            { -1, 10 },
            { 0, 15 },
            { 1, 1 }
        };

        leftRightWeightsClose = new Dictionary<int, int>
        {
            { -1, 10 },
            { 0, 0 },
            { 1, 9 }
        };

    }

    private void Fighter_GotHit(object sender, EventArgs e)
    {
        StopCombo(sender, e);
        // random burst
        if (fighter.HasBurst)
        {
            if (RandomMethods.RANDOM.Next(3) == 0)
            {
                IReadPackage package = new ReadPackage(null, 
                    new List<IReadableGesture>() { new NoGesture()},
                    new List<IButton>() { new SuperDefenseButton() },
                    0);


                bufferedInput = package;
                bufferedAttackTime = 0;
                timesPressedAtComboIndex = 0;
            }
        }
    }

    // some/many attacks that dont need to connect are cancellable starting in their active frames
    // so we advance the combo there too
    private void Fighter_AttackActive(object sender, EventArgs e)
    {
        if (currentCombo != null)
        {
            if (comboIndex >= currentCombo.moves.Count)
            {
                // finished the combo
                currentCombo = null;
                return;
            }
            if (!lastHitConnected)
            {
                if (!currentCombo.moves[comboIndex].needsToConnect)
                {
                    comboIndex += 1;
                    bufferedInput = null;
                    timesPressedAtComboIndex = 0;
                    if (comboIndex >= currentCombo.moves.Count)
                    {
                        // finished the combo
                        currentCombo = null;
                    }
                }
            }
        }
    }

    private void Fighter_Parried(object sender, EventArgs e)
    {
        // reset buffer to give opporunity to pick the new move
        bufferedInput = null;
        currentCombo = null;
        StartNewCombo();
    }

    private void StopCombo(object sender, EventArgs e)
    {
        // dont try to continue this combo
        currentCombo = null;
    }

    private void Fighter_HitConnected(object sender, EventArgs e)
    {
        lastHitConnected = true;
        comboIndex += 1;
        timesPressedAtComboIndex = 0;
        if (currentCombo != null)
        {
            if (comboIndex >= currentCombo.moves.Count)
            {
                // finished the combo
                currentCombo = null;
            }
        }
        // reset buffer to give opporunity to pick the new move
        bufferedInput = null;
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
            if (!lastHitConnected)
            {
                if (currentCombo.moves[comboIndex].needsToConnect)
                {
                    //dropped the combo
                    currentCombo = null;
                }
                else
                {
                    comboIndex += 1;
                    bufferedInput = null;
                    timesPressedAtComboIndex = 0;
                    if (comboIndex >= currentCombo.moves.Count)
                    {
                        // finished the combo
                        currentCombo = null;
                    }
                }
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


        // limit to 60 ticks per second
        timer += Time.unscaledDeltaTime;
        if (timer < timerMax)
        {
            return false;
        }

        timer -= timerMax;

        isClose = Mathf.Abs(fighter.otherFighter.transform.position.x - fighter.transform.position.x) <= closeDistance;

        if (RandomMethods.RANDOM.Next(4) == 0)
        {
            if (RandomMethods.RANDOM.Next(2) == 0)
            {
                if (currentCombo == null)
                {
                    var mashRate = isClose ? mashRateClose : mashRateFar;

                    if (RandomMethods.RANDOM.Next(mashRate) == 0)
                    {
                        StartNewCombo();
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

                        //Debug.Log($"{fighter.name} Attempting move: {gestures[0]} {buttons[0]}");

                        bufferedInput = package;
                        bufferedAttackTime = 0;
                        lastHitConnected = false;

                        timesPressedAtComboIndex += 1;

                        if (timesPressedAtComboIndex > 5)
                        {
                            comboIndex = 0;
                            currentCombo = null;
                        }
                    }

                }

            }

            ChooseMoveDirections();
        }
        return false;
    }

    Dictionary<CpuCombo, int> combosWeighted;
    public void StartNewCombo()
    {
        combosWeighted = new Dictionary<CpuCombo, int>();

        foreach(var c in combos)
        {
            if (c.CanExecute(fighter))
            {
                combosWeighted.Add(c, c.weight);
            }
        }

        currentCombo = RandomMethods.ChooseWeighted(combosWeighted);

        if (currentCombo != null)
        {
            // found a combo we can execute
            // start it at 0
            comboIndex = 0;
            timesPressedAtComboIndex = 0;
        }
    }

    public void ChooseMoveDirections()
    {
        var leftRightWeights = isClose ? leftRightWeightsClose : leftRightWeightsFar;
        var upDownWeights = isClose ? upDownWeightsClose : upDownWeightsFar;

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