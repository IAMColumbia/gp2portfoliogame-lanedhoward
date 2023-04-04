using CommandInputReaderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static CommandInputReaderLibrary.Directions;

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
    protected ContactFilter2D filter;

    private Color inactiveColor = Color.yellow;
    private Color collisionOpenColor = Color.red;
    private Color collidingColor = Color.magenta;

    private ColliderState _state = ColliderState.Closed;
    public bool OpenForCollision;

    private IHitboxResponder responder;

    private void Start()
    {
        SetResponder(fighterParent);
        filter = new ContactFilter2D();
        filter.NoFilter();
    }

    public void CheckHitbox()
    {
        Vector3 actingOffset = new Vector3(offset.x * transform.lossyScale.x, offset.y * transform.lossyScale.y, offset.z * transform.lossyScale.z);

        
        List<Collider2D> colliders = new List<Collider2D>();
        
        //Collider2D collider =
        Physics2D.OverlapBox(transform.position + actingOffset, halfExtents*2, 0, filter, colliders);

        if (colliders.Count > 0)
        {

            _state = ColliderState.Colliding;

            foreach (Collider2D c in colliders)
            {
                responder?.CollidedWith(c);
            }

        }
        else
        {
            _state = ColliderState.Open;
        }

    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = CheckGizmoColor();
        if (OpenForCollision)
        {
            Gizmos.color = Color.red;

            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

            Gizmos.DrawWireCube(Vector3.zero + offset, new Vector3(halfExtents.x * 2, halfExtents.y * 2, halfExtents.z * 2)); // Because size is halfExtents


        }
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
