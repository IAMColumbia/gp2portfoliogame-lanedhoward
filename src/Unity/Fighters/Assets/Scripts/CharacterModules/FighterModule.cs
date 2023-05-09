using CommandInputReaderLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu]
public class FighterModule : CharacterModule
{
    public override List<GameAttack> GetGameAttacks()
    {
        return FighterAttacks.GetFighterAttacks();
    }
    public override List<IReadableGesture> GetPossibleGestures()
    {
        return FighterGestures.GetDefaultGestures();
    }
}
