using CommandInputReaderLibrary;
using CommandInputReaderLibrary.Gestures;
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
        return BlackhandAttacks.GetGestures();
    }

    public override List<CpuCombo> GetCpuCombos()
    {
        List<CpuCombo> baseCombos = base.GetCpuCombos();

        List<CpuCombo> combos = new List<CpuCombo>()
        {
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackA(), true),
                    new CpuMove(new NoGesture(), new AttackB(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new DragonPunch(), new AttackC(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new CrouchGesture(), new AttackA(), true),
                    new CpuMove(new CrouchGesture(), new AttackB(), true),
                    new CpuMove(new CrouchGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleBack(), new AttackB(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new CrouchGesture(), new AttackB(), true),
                    new CpuMove(new CrouchGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleBack(), new AttackD(), true),
                    new CpuMove(new NoGesture(), new AttackB(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleBack(), new AttackB(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackB(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleBack(), new AttackD(), true),
                    new CpuMove(new NoGesture(), new AttackB(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new DragonPunch(), new AttackC(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleForward(), new AttackB(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new CrouchGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleBack(), new AttackD(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new QuarterCircleBack(), new AttackD(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new QuarterCircleForward(), new AttackD(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new QuarterCircleForward(), new AttackB(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new QuarterCircleBack(), new AttackB(), false)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new DragonPunch(), new AttackC(), true)
                }
            }

        };

        combos.AddRange(baseCombos);
        return combos;
    }
}
