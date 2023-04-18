using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(FighterMain))]
public class FighterInspector : Editor
{
    private string[] pastStates;

    public FighterInspector()
    {
        pastStates = new string[5];
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FighterMain fighter = (FighterMain)target;

        if (fighter != null)
        {
            if (fighter.currentState != null)
            {
                string name = fighter.currentState.GetType().Name;

                if (name != pastStates[0])
                {
                    AddPastState(name);
                }

                

            }
        }

        DrawPastStates();

        DrawCurrentAttack(fighter);
    }

    private void DrawPastStates()
    {
        EditorGUILayout.LabelField("Recent States");
        for (int i = 0; i < pastStates.Length; i++)
        {
            EditorGUILayout.LabelField(i + ".  " + pastStates[i]);
        }
    }

    private void AddPastState(string name)
    {
        for (int i = pastStates.Length-1; i > 0; i--)
        {
            pastStates[i] = pastStates[i - 1];
        }
        pastStates[0] = name;
    }

    private void DrawCurrentAttack(FighterMain fighter)
    {
        string attackName = "";
        if (fighter.currentAttack != null)
        {
            attackName = fighter.currentAttack.GetType().Name;
        }
        EditorGUILayout.LabelField("Current Attack: " + attackName);
    }
}
