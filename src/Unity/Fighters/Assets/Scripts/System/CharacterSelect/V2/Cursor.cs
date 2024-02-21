using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    public PlayerInput playerInput;

    public float moveSpeed = 10f;

    public TextMeshPro PlayerText;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;

    public HumanPlayerConfig humanPlayerConfig;

    public GamePlayerSlot gamePlayerSlot;

    public CharacterSelectUIManager uiManager;

    public Token token;

    public Canvas canvas;

    public SpriteRenderer tokenSprite;

    public void SetGraphicRaycaster(GraphicRaycaster raycaster)
    {
        this.raycaster = raycaster;
    }

    public void SetPlayerInput(PlayerInput pi)
    {
        playerInput = pi;
    }

    private void Awake()
    {
        pointerEventData = new PointerEventData(null);
    }

    private void Start()
    {
        //GamePlayerManager should know about the PI by now, so we should be able to set our HumanPlayerConfig
        GamePlayerManager.Instance.SetUpCursor(this);
        uiManager.SetUpCursor(this);

        gamePlayerSlot.SetHumanPlayerConfig(humanPlayerConfig);

        PickUpToken(gamePlayerSlot.token);

        if (PlayerText != null)
        {
            PlayerText.text = $"P{gamePlayerSlot.PlayerSlotIndex + 1}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput != null)
        {
            Vector3 move = playerInput.actions["Move"].ReadValue<Vector2>();
            
            move.Normalize();

            if (move != Vector3.zero)
            {
                transform.position += moveSpeed * Time.deltaTime * move;
            }


            if (playerInput.actions["Submit"].WasPressedThisFrame())
            {
                bool dropToken = true;

                pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);

                List<RaycastResult> results = new();
                raycaster.Raycast(pointerEventData, results);


                if (results.Count > 0)
                {
                    List<CursorButton> buttons = new List<CursorButton>();
                    
                    foreach (var r in results)
                    {
                        CursorButton button = r.gameObject.GetComponent<CursorButton>();
                        if (button != null) buttons.Add(button);
                        //if (button != null) button.Interact(this);
                    }

                    if (buttons.Count > 0)
                    {
                        dropToken = buttons.OrderByDescending(b => b.Priority).First().Interact(this);
                    }
                }

                if (token != null && dropToken == true)
                {
                    DropToken();
                }

            }

        }
    }

    public void PickUpToken(Token t)
    {
        if (token != null) return;
        token = t;
        token.gameObject.SetActive(false);
        tokenSprite.color = t.color;
        tokenSprite.gameObject.SetActive(true);

        token.slot.ClearCharacter();

    }

    public void DropToken()
    {
        token.rect.anchoredPosition = canvas.WorldToCanvasPosition(transform.position);
        token.gameObject.SetActive(true);
        token = null;
        tokenSprite.gameObject.SetActive(false);
    }
}
