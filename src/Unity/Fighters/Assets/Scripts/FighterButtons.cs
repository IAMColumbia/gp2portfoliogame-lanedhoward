using CommandInputReaderLibrary;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackA : Button
{
    public AttackA()
    {
        Priority = 80;
    }
}

public class AttackB : Button
{
    public AttackB()
    {
        Priority = 60;
    }
}

public class AttackC : Button
{
    public AttackC()
    {
        Priority = 40;
    }
}

public class AttackD : Button
{
    public AttackD()
    {
        Priority = 20;
    }
}

public class DashMacro : Button
{
    public DashMacro()
    {
        Priority = 100;
    }
}

public class SpecialButton : Button
{
    public SpecialButton()
    {
        Priority = 120;
    }
}

public class SuperButton : Button
{
    public SuperButton()
    {
        Priority = 150;
    }
}