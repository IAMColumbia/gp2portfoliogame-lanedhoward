using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    public RectTransform rect;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    public void GetPickedUpBy(Cursor cursor)
    {
        cursor.SetToken(this);
    }
}
