using CommandInputReaderLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu]
public class MrsHiveModule : CharacterModule
{
    public override List<GameAttack> GetGameAttacks()
    {
        return MrsHiveAttacks.GetMrsHiveAttacks();
    }
    public override List<IReadableGesture> GetPossibleGestures()
    {
        return FighterGestures.GetDefaultGestures();
    }
}
