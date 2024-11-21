using CommandInputReaderLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu]
public class KnightModule : CharacterModule
{
    public override List<GameAttack> GetGameAttacks()
    {
        return KnightAttacks.GetKnightAttacks();
    }
    public override List<IReadableGesture> GetPossibleGestures()
    {
        return FighterGestures.GetDefaultGestures();
    }
}
