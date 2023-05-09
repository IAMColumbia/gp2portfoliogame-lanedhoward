using CommandInputReaderLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu]
public class BlackhandModule : CharacterModule
{
    public override List<GameAttack> GetGameAttacks()
    {
        return BlackhandAttacks.GetBlackhandAttacks();
    }
    public override List<IReadableGesture> GetPossibleGestures()
    {
        return FighterGestures.GetDefaultGestures();
    }
}
