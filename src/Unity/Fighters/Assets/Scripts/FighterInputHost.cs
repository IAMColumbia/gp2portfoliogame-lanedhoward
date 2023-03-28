using CommandInputReaderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FighterInputHost : MonoBehaviour, IInputHost
{
    private InputActionAsset inputAsset;
    private InputActionMap player;
    private InputAction move;
    private InputAction attackA;
    private InputAction attackB;
    private InputAction attackC;
    private InputAction dashMacro;


    void Awake()
    {
        inputAsset = this.GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Fighter");
    }

    private void OnEnable()
    {
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

        return p;
    }

}
