using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CursorButton : MonoBehaviour
{
    public int Priority;
    public bool DropToken;
    public UnityEvent<Cursor> OnInteract;
    public virtual bool Interact(Cursor cursor)
    {
        Debug.Log($"{cursor.name} interacted with {this.name}!");
        OnInteract?.Invoke(cursor);
        return DropToken;
    }
}
