using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CursorButton : MonoBehaviour
{
    public UnityEvent<Cursor> OnInteract;
    public virtual void Interact(Cursor cursor)
    {
        Debug.Log($"{cursor.name} interacted with {this.name}!");
        OnInteract?.Invoke(cursor);
    }
}
