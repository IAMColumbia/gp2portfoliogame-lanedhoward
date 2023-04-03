using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum ColliderState
{
    Closed,
    Open,
    Colliding
}

public class Hitbox : MonoBehaviour
{
    public FighterMain fighterParent;

    public int id;

    public Vector3 offset;
    public Vector3 halfExtents;
    public LayerMask mask;

    public Color inactiveColor;
    public Color collisionOpenColor;
    public Color collidingColor;

    private ColliderState _state = ColliderState.Closed;
    public bool OpenForCollision;

    private IHitboxResponder responder;

    public void CheckHitbox()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + offset, halfExtents, transform.rotation, mask);

        if (colliders.Length > 0)
        {

            _state = ColliderState.Colliding;

            responder?.CollidedWith(colliders);

        }
        else
        {
            _state = ColliderState.Open;
        }

    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = CheckGizmoColor();
        Gizmos.color = collidingColor;

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

        Gizmos.DrawCube(Vector3.zero + offset, new Vector3(halfExtents.x * 2, halfExtents.y * 2, halfExtents.z * 2)); // Because size is halfExtents

    }

    private void FixedUpdate()
    {
        if (!OpenForCollision) 
        {
            _state = ColliderState.Closed;
            return; 
        }
        if (_state == ColliderState.Colliding) return;

        CheckHitbox();
        

    }

    private Color CheckGizmoColor()
    {
        switch (_state)
        {
            case ColliderState.Closed:
                return inactiveColor;

            default:
            case ColliderState.Open:
                return collisionOpenColor;

            case ColliderState.Colliding:
                return collidingColor;
        }

    }

    public void SetResponder(IHitboxResponder _responder)
    {
        responder = _responder;
    }
}
