using CommandInputReaderLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class FighterMain : MonoBehaviour
{
    public FighterInputReceiver inputReceiver;
    public FighterAnimator fighterAnimator;

    public Rigidbody2D fighterRigidbody;

    public FighterState currentState;

    public Neutral neutral;

    [Header("Movement Values")]
    public float walkAccel;
    public float walkMaxSpeed;
    public float groundFriction;
    public float velocityToStopMoveAnimation;


    void Start()
    {
        var inputHost = new FighterInputHost(GetComponent<PlayerInput>());
        var inputReader = new InputReader(inputHost);
        inputReader.SetPossibleGestures(CommandInputReaderLibrary.Gestures.DefaultGestures.GetDefaultGestures());
        inputReceiver = new FighterInputReceiver(inputHost, inputReader);

        fighterRigidbody = GetComponent<Rigidbody2D>();

        fighterAnimator = new FighterAnimator(this);
        fighterAnimator.velocityToStopMovingAnim = velocityToStopMoveAnimation;

        neutral = new Neutral(this);

        //currentState = neutral;
        SwitchState(neutral);
    }


    void FixedUpdate()
    {
        CheckForInputs();

        //HandleInputs();

        //UpdateTimers();

        //CheckForGroundedness();

        DoCurrentState();
    }

    private void CheckForInputs()
    {
        inputReceiver.CheckForInputs();
    }

    public void SwitchState(FighterState newState)
    {
        currentState?.ExitState();

        currentState = newState;

        currentState.EnterState();
    }

    public void DoCurrentState()
    {
        currentState.DoState();
    }
}
