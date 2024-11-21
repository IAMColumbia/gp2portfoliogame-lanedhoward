using CommandInputReaderLibrary;
using CommandInputReaderLibrary.Gestures;
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
        return MrsHiveAttacks.GetGestures();
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
                    new CpuMove(new HalfCircleForward(), new AttackB(), true)
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
                    new CpuMove(new DownDownGesture(), new AttackB(), true),
                    new CpuMove(new NoGesture(), new AttackA(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true)
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
                    new CpuMove(new QuarterCircleForward(), new AttackA(), false)
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
                    new CpuMove(new QuarterCircleForward(), new AttackA(), false),
                    new CpuMove(new NoGesture(), new AttackB(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleBack(), new AttackA(), true)
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
                    new CpuMove(new HalfCircleForward(), new AttackB(), true),
                    new CpuMove(new NoGesture(), new AttackB(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleForward(), new AttackA(), false)
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
                    new CpuMove(new QuarterCircleForward(), new AttackA(), false)
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
                    new CpuMove(new QuarterCircleForward(), new AttackA(), false)
                }
            },
            // raw specials
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new HalfCircleForward(), new AttackB(), true)
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
                    new CpuMove(new DownDownGesture(), new AttackB(), false)
                }
            },
            new CpuCombo()
            {
                weight = 300,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new QuarterCircleForward(), new AttackA(), false)
                }
            },

            // honey moves / combos
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, false),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new DashMacro(), false)
                }
            },
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, false),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new DashMacro(), false),
                    new CpuMove(new NoGesture(), new AttackA(), true)
                }
            },
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, false),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new DashMacro(), false),
                    new CpuMove(new NoGesture(), new AttackB(), true)
                }
            },
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, false),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new DashMacro(), false),
                    new CpuMove(new NoGesture(), new AttackC(), true)
                }
            },
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, false),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new DashMacro(), false),
                    new CpuMove(new CrouchGesture(), new AttackC(), true)
                }
            },
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, false),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackA(), true),
                    new CpuMove(new CrouchGesture(), new AttackC(), true)
                }
            },
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, false),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackB(), true),
                    new CpuMove(new CrouchGesture(), new AttackC(), true)
                }
            },
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, false),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new CrouchGesture(), new AttackC(), true)
                }
            },

            //grounded honey moves
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new DownDownGesture(), new AttackA(), false)
                }
            },
            new CpuCombo()
            {
                weight = 20000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new DownDownGesture(), new AttackA(), false),
                    new CpuMove(new DownDownGesture(), new AttackA(), false)
                }
            },
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new QuarterCircleBack(), new AttackA(), false)
                }
            },
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new QuarterCircleForward(), new AttackC(), false)
                }
            },

            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackA(), true),
                    new CpuMove(new NoGesture(), new AttackB(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleForward(), new AttackC(), false)
                }
            },
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new CrouchGesture(), new AttackA(), true),
                    new CpuMove(new DownDownGesture(), new AttackB(), true),
                    new CpuMove(new NoGesture(), new AttackA(), true),
                    new CpuMove(new CrouchGesture(), new AttackC(), true)
                }
            },
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new CrouchGesture(), new AttackA(), true),
                    new CpuMove(new CrouchGesture(), new AttackB(), true),
                    new CpuMove(new CrouchGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleBack(), new AttackA(), true)
                }
            },
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new CrouchGesture(), new AttackB(), true),
                    new CpuMove(new CrouchGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleForward(), new AttackC(), true),
                }
            },
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackB(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleForward(), new AttackC(), true)
                }
            },
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleForward(), new AttackC(), true)
                }
            },
            new CpuCombo()
            {
                weight = 100000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new HasStockCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new CrouchGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleForward(), new AttackC(), true)
                }
            },

        };

        combos.AddRange(baseCombos);
        return combos;
    }
}
