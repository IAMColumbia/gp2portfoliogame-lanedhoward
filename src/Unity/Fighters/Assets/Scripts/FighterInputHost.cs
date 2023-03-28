using CommandInputReaderLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FighterInputHost : IInputHost
{
    private PlayerInput playerInput;
    private InputActionAsset inputAsset;
    private InputActionMap player;
    private InputAction move;
    private InputAction attackA;
    private InputAction attackB;
    private InputAction attackC;
    private InputAction dashMacro;

    public FighterInputHost(PlayerInput _playerInput)
    {
        playerInput = _playerInput;
        Initialize();
    }

    protected void Initialize()
    {
        inputAsset = playerInput.actions;
        player = inputAsset.FindActionMap("Fighter");
        player.Enable();
        move = player.FindAction("Move");
        attackA = player.FindAction("AttackA");
        attackA = player.FindAction("AttackA");
        attackA = player.FindAction("AttackB");
        attackA = player.FindAction("AttackC");
        dashMacro = player.FindAction("DashMacro");
    }

    public IHostPackage GetCurrentInputs()
    {
        var v = move.ReadValue<Vector2>();

        var p = new HostPackage();
        p.UpDown = (int)v.y;
        p.LeftRight = (int)v.x;

        AddButtonToHostPackage<AttackA>(p, attackA);
        AddButtonToHostPackage<AttackB>(p, attackB);
        AddButtonToHostPackage<AttackC>(p, attackC);
        AddButtonToHostPackage<DashMacro>(p, dashMacro);

        return p;
    }

    private void AddButtonToHostPackage<ButtonType>(HostPackage p, InputAction inputAction) where ButtonType: IButton, new()
    {
        if (inputAction.triggered)
        {
            p.Buttons.Add(new ButtonType() { State = IButton.ButtonState.Pressed });
        }
    }

}
