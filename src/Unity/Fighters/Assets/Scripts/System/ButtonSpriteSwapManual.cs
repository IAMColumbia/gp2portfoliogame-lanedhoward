using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class ButtonSpriteSwapManual : MonoBehaviour
{
    public Button button;
    public MultiplayerEventSystem eventSystem;
    bool selected = false;
    public Sprite selectedSprite, normalSprite;

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            if (eventSystem.currentSelectedGameObject != gameObject)
            {
                // deselect
                selected = false;   
                button.image.sprite = normalSprite;
            }
        }
        else
        {
            if (eventSystem.currentSelectedGameObject == gameObject)
            {
                // select
                selected = true;
                button.image.sprite = selectedSprite;

            }
        }
    }
    
}
