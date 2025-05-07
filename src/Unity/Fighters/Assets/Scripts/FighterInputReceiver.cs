using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandInputReaderLibrary;
using System;
using System.Linq;

public class FighterInputReceiver : IInputReceiver
{
    protected FighterMain fighter;
    protected FighterInputHost inputHost;
    protected InputReader inputReader;

    protected IButton lastButton;
    protected float lastButtonTime;
    protected float buttonBufferTimeMax;

    public int UpDown;
    public int LeftRight;

    public IReadPackage bufferedInput;
    public float bufferedAttackTime;
    public float bufferedAttackTimeMax;

    public FighterInputReceiver(FighterMain _fighter, FighterInputHost _inputHost, InputReader _inputReader)
    {
        fighter = _fighter;
        inputHost = _inputHost;
        inputReader = _inputReader;

        UpDown = 0;
        LeftRight = 0;

        lastButton = null;
        lastButtonTime = 0;
        buttonBufferTimeMax = InputReader.TimeBetweenNonSequentialInputs;

        bufferedAttackTime = 0f;
        bufferedAttackTimeMax = 0.2f; // TODO: Convert this into "frames" like the input reader uses
    }

    public virtual void SetPossibleGestures(List<IReadableGesture> possibleGestures)
    {
        inputReader.SetPossibleGestures(possibleGestures);
    }

    public virtual bool CheckForInputs()
    {
        ManageBuffer();

        IReadPackage package = inputReader.Tick(TimeSpan.FromSeconds(Time.unscaledDeltaTime));

        if (package == null)
        {
            return false;
        }

        UpDown = package.mostRecentInputs.UpDown;
        LeftRight = package.mostRecentInputs.LeftRight;
        
        // then we'll handle gestures and buttons and such

        //if (package.buttons.Count == 0)
        //{
            // no buttons currently pressed, so we check if there were any buttons pressed in the buffered input
            // we do this so that you can press your button a little bit before completing a gesture
            // and have it register still

            // might still need to rework this to have proper kara canceling
            // and also if i decide to have 2 button attacks
            /*
            if (bufferedInput != null)
            {
                if (package.TimeReceived - bufferedInput.TimeReceived <= buttonBufferTimeMax)
                {
                    package.buttons.AddRange(bufferedInput.buttons);
                    package.buttons.OrderBy(b => b.Priority);
                    package.gestures.AddRange(bufferedInput.gestures);
                    package.gestures.OrderBy(g => g.Priority);
                }
            }
            */
            
        //}


        if (package.buttons.Count > 0 && package.buttons.Any(b => b.State == IButton.ButtonState.Pressed))
        {
            bufferedInput = package;
            bufferedAttackTime = 0;
        }

        return true;
    }

    protected void ManageBuffer()
    {
        if (bufferedInput != null)
        {
            bufferedAttackTime += Time.deltaTime;

            if (bufferedAttackTime >= bufferedAttackTimeMax)
            {
                bufferedInput = null;
            }
        }
    }

    public GameAttack ParseAttack(IReadPackage package, bool isKara = false)
    {
        if (package.buttons.Count <= 0)
        {
            if (package.gestures.Any(g => g is IStandaloneGesture))
            {
                return ParseAttackStandalone(package);
            }
            return null;
        }

        NoGesture noGesture = new NoGesture();

        IGesture currentGesture = noGesture;

        IButton currentButton = package.buttons.FirstOrDefault(b => b.State == IButton.ButtonState.Pressed);

        bool combinedButtons = false;

        if (package.buttons.Count > 1)
        {
            // 2 buttons, check for 2 button commands

            // special + any attack button == super button

            // any 2 attack buttons == super defense button

            if (package.buttons.Where(b => b is AttackA || b is AttackB || b is AttackC).Count() >= 2)
            {
                currentButton = new SuperDefenseButton();
                combinedButtons = true;
            }
            else if (package.buttons.Where(b => b is AttackA || b is AttackB || b is AttackC || b is SpecialButton).Count() >= 2)
            {
                currentButton = new SuperButton();
                combinedButtons = true;
            }

        }

        // expected kara but didnt acutally get a kara input
        if (isKara && !combinedButtons) return null;

        GameMoveInput currentMoveInput = new GameMoveInput(currentGesture, currentButton);


        for (int i = 0; i < package.gestures.Count; i++)
        {
            if (package.gestures.Count > 0)
            {
                currentGesture = package.gestures[i];
            }

            currentMoveInput.gesture = currentGesture;

            foreach (GameAttack attack in fighter.fighterAttacks)
            {
                if (attack.CanExecute(currentMoveInput, fighter))
                {
                    // found our attack!
                    return attack;
                }
            }
        }
        return null;
    }

    protected GameAttack ParseAttackStandalone(IReadPackage package)
    {
        List<IStandaloneGesture> standalones = package.gestures.Where(g => g is IStandaloneGesture).Cast<IStandaloneGesture>().ToList();

        

        foreach (var s in standalones)
        {
            GameMoveInput moveInput = null;

            // TODO: find a better way than hard coding these
            if (s is BackDashGesture) 
            {
                moveInput = new GameMoveInput(new BackGesture(), new DashMacro());
            }
            if (s is DashGesture)
            {
                moveInput = new GameMoveInput(new ForwardGesture(), new DashMacro());
            }
            if (s is NeutralDashGesture)
            {
                moveInput = new GameMoveInput(new NeutralGesture(), new DashMacro());
            }


            if (moveInput != null)
            {
                foreach (GameAttack attack in fighter.fighterAttacks)
                {
                    if (attack.CanExecute(moveInput, fighter))
                    {
                        // found our attack!
                        return attack;
                    }
                }

            }
        }
        return null;
    }

    public virtual void UpdateFacingDirection()
    {
        if (inputReader.GetFacingDirection() != fighter.facingDirection)
        {
            inputReader.ChangeFacingDirection(fighter.facingDirection);

            // if we had a move buffered, check its gestures again for the new direction
            if (bufferedInput != null)
            {
                inputReader.ReReadGestures(bufferedInput);
            }
        }
    }

    public virtual Directions.Direction GetDirection()
    {
        return Directions.GetDirectionFacingForward(this.UpDown, this.LeftRight, fighter.facingDirection);
    }
}
