using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitboxResponder
{
    public bool CollidedWith(Collider2D collider);
}
