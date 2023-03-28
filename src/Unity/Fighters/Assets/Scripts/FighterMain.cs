using CommandInputReaderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class FighterMain : MonoBehaviour
{
    protected FighterInputHost inputHost;
    protected InputReader inputReader;
    // Start is called before the first frame update
    void Start()
    {
        inputHost = new FighterInputHost(GetComponent<PlayerInput>());
        inputReader = new InputReader(inputHost);
        inputReader.SetPossibleGestures(CommandInputReaderLibrary.Gestures.DefaultGestures.GetDefaultGestures());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
