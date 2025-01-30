using CommandInputReaderLibrary;
using CommandInputReaderLibrary.Gestures;
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
        return FighterAttacks.GetGestures();
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
                    new CpuMove(new NoGesture(), new AttackA(), true),
                    new CpuMove(new CrouchGesture(), new AttackB(), true),
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
                    new CpuMove(new CrouchGesture(), new AttackA(), true),
                    new CpuMove(new CrouchGesture(), new AttackB(), true),
                    new CpuMove(new CrouchGesture(), new AttackC(), true),
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
                    new CpuMove(new CrouchGesture(), new AttackB(), true),
                    new CpuMove(new CrouchGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleBack(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), true),
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
                    new CpuMove(new CrouchGesture(), new AttackB(), true),
                    new CpuMove(new QuarterCircleForward(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
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
                    new CpuMove(new CrouchGesture(), new AttackC(), true),
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
                    new CpuMove(new NoGesture(), new AttackB(), true),
                    new CpuMove(new QuarterCircleForward(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleBack(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackB(), true)
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
                    new CpuMove(new NoGesture(), new AttackA(), true),
                    new CpuMove(new NoGesture(), new AttackB(), true),
                    new CpuMove(new QuarterCircleForward(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleBack(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackB(), true)
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
                    new CpuMove(new QuarterCircleBack(), new AttackC(), true)
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
                    new CpuMove(new QuarterCircleBack(), new AttackC(), true)
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
                    new CpuMove(new QuarterCircleForward(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true)
                }
            },
            new CpuCombo()
            {
                weight = 150,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new QuarterCircleForward(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackB(), true)
                }
            },
            new CpuCombo()
            {
                weight = 150,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new QuarterCircleBack(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackB(), true)
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
                    new CpuMove(new QuarterCircleForward(), new AttackA(), false),
                    new CpuMove(new BackGesture(), new AttackC(), true)
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
                    new CpuMove(new QuarterCircleBack(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackA(), false)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new MeterCostCondition(null, 200f)

                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackA(), true),
                    new CpuMove(new NoGesture(), new AttackB(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new SuperButton(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new MeterCostCondition(null, 200f)

                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new CrouchGesture(), new AttackA(), true),
                    new CpuMove(new CrouchGesture(), new AttackB(), true),
                    new CpuMove(new CrouchGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new SuperButton(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new MeterCostCondition(null, 200f)

                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new CrouchGesture(), new AttackA(), true),
                    new CpuMove(new CrouchGesture(), new AttackB(), true),
                    new CpuMove(new CrouchGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleForward(), new AttackA(), false),
                    new CpuMove(new NoGesture(), new SuperButton(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new MeterCostCondition(null, 200f)

                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackA(), true),
                    new CpuMove(new NoGesture(), new AttackB(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleForward(), new AttackA(), false),
                    new CpuMove(new NoGesture(), new SuperButton(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new MeterCostCondition(null, 200f)

                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new QuarterCircleForward(), new AttackA(), false),
                    new CpuMove(new NoGesture(), new SuperButton(), true)
                }
            },
        };

        combos.AddRange(baseCombos);
        return combos;
    }
}
