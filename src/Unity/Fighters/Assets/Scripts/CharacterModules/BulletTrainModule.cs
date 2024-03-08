using CommandInputReaderLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu]
public class BulletTrainModule : CharacterModule
{
    public override List<GameAttack> GetGameAttacks()
    {
        return BulletTrainAttacks.GetBulletTrainAttacks();
    }
    public override List<IReadableGesture> GetPossibleGestures()
    {
        return BulletTrainAttacks.GetGestures();
    }
}
