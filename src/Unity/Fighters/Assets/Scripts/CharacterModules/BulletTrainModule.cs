using CommandInputReaderLibrary;
using CommandInputReaderLibrary.Gestures;
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
                    new CpuMove(new QuarterCircleForward(), new AttackC(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false)

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
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new QuarterCircleForward(), new AttackC(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false)
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
                    new CpuMove(new QuarterCircleForward(), new AttackC(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false)
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
                    new CpuMove(new QuarterCircleForward(), new AttackC(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false)

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
                    new CpuMove(new QuarterCircleForward(), new AttackC(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false)
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
                    new CpuMove(new QuarterCircleForward(), new AttackC(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false)
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
                    new CpuMove(new QuarterCircleForward(), new AttackC(), false),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false)
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
                    new CpuMove(new QuarterCircleForward(), new AttackC(), false),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new QuarterCircleBack(), new AttackC(), false)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new AmmoNotFullCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new QuarterCircleForward(), new AttackC(), false),
                    new CpuMove(new NoGesture(), new AttackC(), false),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new QuarterCircleBack(), new AttackC(), false)
                }
            },
            new CpuCombo()
            {
                weight = 1000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new AmmoEmptyCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new QuarterCircleForward(), new AttackC(), false),
                    new CpuMove(new NoGesture(), new AttackC(), false),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new QuarterCircleBack(), new AttackC(), false)
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
                    new CpuMove(new QuarterCircleForward(), new AttackC(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false)
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
                    new CpuMove(new QuarterCircleForward(), new AttackC(), false)
                }
            },
            // in gun stance
            new CpuCombo()
            {
                weight = 300000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new LogicalOrCondition(null,
                        new FollowUpCondition(null, typeof(GunStance)),
                        new FollowUpCondition(null, typeof(GunStanceAttack)))
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new QuarterCircleBack(), new AttackC(), false)
                }
            },
            new CpuCombo() // put your gun away if youre far
            {
                weight = 8000000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new LogicalOrCondition(null,
                        new FollowUpCondition(null, typeof(GunStance)),
                        new FollowUpCondition(null, typeof(GunStanceAttack))),
                    new FarCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new QuarterCircleBack(), new AttackC(), false)
                }
            },
            new CpuCombo()
            {
                weight = 300000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new LogicalOrCondition(null,
                        new FollowUpCondition(null, typeof(GunStance)),
                        new FollowUpCondition(null, typeof(GunStanceAttack))),
                    new AmmoNotFullCondition(null)

                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new NoGesture(), new AttackA(), false)
                }
            },
            new CpuCombo() // reload while empty, much more important
            {
                weight = 6000000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new LogicalOrCondition(null,
                        new FollowUpCondition(null, typeof(GunStance)),
                        new FollowUpCondition(null, typeof(GunStanceAttack))),
                    new AmmoEmptyCondition(null)

                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new NoGesture(), new AttackA(), false)
                }
            },
            new CpuCombo()
            {
                weight = 600000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new LogicalOrCondition(null,
                        new FollowUpCondition(null, typeof(GunStance)),
                        new FollowUpCondition(null, typeof(GunStanceAttack))),
                    new AmmoNotFullCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackA(), false)
                }
            },
            new CpuCombo()
            {
                weight = 1000000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new LogicalOrCondition(null,
                        new FollowUpCondition(null, typeof(GunStance)),
                        new FollowUpCondition(null, typeof(GunStanceAttack)))
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false)
                }
            },
            new CpuCombo() // PLEASE dash closer when far
            {
                weight = 100000000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new LogicalOrCondition(null,
                        new FollowUpCondition(null, typeof(GunStance)),
                        new FollowUpCondition(null, typeof(GunStanceAttack))),
                    new FarCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false)
                }
            },
            new CpuCombo() // PLEASE dash closer when far
            {
                weight = 100000000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new LogicalOrCondition(null,
                        new FollowUpCondition(null, typeof(GunStance)),
                        new FollowUpCondition(null, typeof(GunStanceAttack))),
                    new FarCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true)
                }
            },
            new CpuCombo() // PLEASE dash closer when far
            {
                weight = 100000000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new LogicalOrCondition(null,
                        new FollowUpCondition(null, typeof(GunStance)),
                        new FollowUpCondition(null, typeof(GunStanceAttack))),
                    new FarCondition(null)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new NoGesture(), new AttackC(), true)
                }
            },
            new CpuCombo()
            {
                weight = 1000000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new LogicalOrCondition(null,
                        new FollowUpCondition(null, typeof(GunStance)),
                        new FollowUpCondition(null, typeof(GunStanceAttack)))
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true)
                }
            },
            new CpuCombo()
            {
                weight = 300000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new LogicalOrCondition(null,
                        new FollowUpCondition(null, typeof(GunStance)),
                        new FollowUpCondition(null, typeof(GunStanceAttack)))
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new BackGesture(), new DashMacro(), false)
                }
            },
            new CpuCombo()
            {
                weight = 1000000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new LogicalOrCondition(null,
                        new FollowUpCondition(null, typeof(GunStance)),
                        new FollowUpCondition(null, typeof(GunStanceAttack)))
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true)
                }
            },
            new CpuCombo()
            {
                weight = 1000000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new LogicalOrCondition(null,
                        new FollowUpCondition(null, typeof(GunStance)),
                        new FollowUpCondition(null, typeof(GunStanceAttack)))
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new NoGesture(), new AttackA(), false)
                }
            },
            new CpuCombo()
            {
                weight = 300000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new LogicalOrCondition(null,
                        new FollowUpCondition(null, typeof(GunStance)),
                        new FollowUpCondition(null, typeof(GunStanceAttack)))
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new BackGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new ForwardOrNeutralGesture(), new DashMacro(), false),
                    new CpuMove(new ForwardGesture(), new AttackC(), true)
                }
            },
            new CpuCombo()
            {
                weight = 300000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new LogicalOrCondition(null,
                        new FollowUpCondition(null, typeof(GunStance)),
                        new FollowUpCondition(null, typeof(GunStanceAttack)))
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackB(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false)
                }
            },
            new CpuCombo()
            {
                weight = 300000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new LogicalOrCondition(null,
                        new FollowUpCondition(null, typeof(GunStance)),
                        new FollowUpCondition(null, typeof(GunStanceAttack)))
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new CrouchGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new ForwardGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackA(), false)
                }
            },
            // if ur in super
            new CpuCombo()
            {
                weight = 100000000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new FollowUpCondition(null, typeof(SuperGunStance))
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),

                }
            },
            new CpuCombo()
            {
                weight =50000000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new FollowUpCondition(null, typeof(SuperGunStance))
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new ForwardGesture(), new DashMacro(), false)

                }
            },
            new CpuCombo()
            {
                weight =50000000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new FollowUpCondition(null, typeof(SuperGunStance))
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new BackGesture(), new DashMacro(), false)

                }
            },
            // super holster
            new CpuCombo()
            {
                weight =20000000,
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true),
                    new FollowUpCondition(null, typeof(SuperGunStance))
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new SpecialButton(), false)

                }
            },
            new CpuCombo()
            {
                weight = 500,
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
                    new CpuMove(new NoGesture(), new SuperButton(), false),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
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
                    new CpuMove(new NoGesture(), new AttackB(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new SuperButton(), false),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true),
                }
            },
        };

        combos.AddRange(baseCombos);
        return combos;
    }
}
