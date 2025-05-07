using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CursorButton : SoundPlayer
{
    public int Priority;
    public bool DropToken;
    public AudioClip clickSound;
    public UnityEvent<Cursor> OnInteract;
    public virtual bool Interact(Cursor cursor)
    {
        OnInteract?.Invoke(cursor);

        if (clickSound != null)
        {
            PlaySound(clickSound);
        }

        return DropToken;
    }
}
