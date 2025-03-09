using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

public class ReverseFireball : Projectile
{
    public float wallXpos;

    public override void StartProjectile()
    {
        if (originalParent == null)
        {
            originalParent = transform.parent;
        }

        bool facingLeft = fighterParent.facingDirection == CommandInputReaderLibrary.Directions.FacingDirection.LEFT;

        UnityEngine.Vector3 newPos = new UnityEngine.Vector3(facingLeft ? -wallXpos : wallXpos, fighterParent.transform.position.y, transform.position.z);

        base.StartProjectile(newPos);
    }
}
