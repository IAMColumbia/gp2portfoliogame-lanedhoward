using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Token : MonoBehaviour
{
    public RectTransform rect;
    public GamePlayerSlot slot;

    public TextMeshProUGUI label;
    public Image image;

    public void SetUpToken(GamePlayerSlot slot)
    {
        this.slot = slot;
        image.material = slot.tokenMaterial;
        label.text = $"P{slot.PlayerSlotIndex + 1}";
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    public void GetPickedUpBy(Cursor cursor)
    {
        cursor.PickUpToken(this);
    }
}
