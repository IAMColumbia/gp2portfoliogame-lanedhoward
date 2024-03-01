using JetBrains.Annotations;
using System;
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

    private Vector2 screenBounds;
    private Vector2 spriteSize;

    public TextMeshPro PlayerText;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;

    public HumanPlayerConfig humanPlayerConfig;

    public GamePlayerSlot gamePlayerSlot;

    public MenuPanels panels;

    public CharacterSelectUIManager uiManager;

    public Token token;

    public Canvas canvas;

    public SpriteRenderer tokenSprite;

    public bool cursorEnabled = false;

    public float cursorEnableDelay;
    public float cursorEnableDelayMax = 0.25f;

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

        SetUpGamePlayerSlot();
        
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        var sprite = GetComponent<SpriteRenderer>();
        spriteSize = new Vector2(sprite.bounds.size.x / 2, sprite.bounds.size.y / 2);

        // wait to enable control, so that you don't instantly drop the token
        cursorEnableDelay = cursorEnableDelayMax;
    }

    public void SetUpGamePlayerSlot()
    {
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
        if (cursorEnableDelay > 0)
        {
            cursorEnableDelay -= Time.deltaTime;

            if (cursorEnableDelay <= 0)
            {
                cursorEnabled = true;
            }
        }

        if (cursorEnabled == false) return;

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

    private void LateUpdate()
    {
        // keep cursor on screen
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, -screenBounds.x + spriteSize.x, screenBounds.x - spriteSize.x);
        viewPos.y = Mathf.Clamp(viewPos.y, -screenBounds.y + spriteSize.y, screenBounds.y - spriteSize.y);
        transform.position = viewPos;
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

    private void OnEnable()
    {
        ControlsPanel.ControlsOpened += OnMenuPanelOpened;
        ControlsPanel.ControlsClosed += OnMenuPanelClosed;
        MovesListPanel.MovesListOpened += OnMenuPanelOpened;
        MovesListPanel.MovesListClosed += OnMenuPanelClosed;

        GamePlayerManager.PlayersSwapped += GamePlayerManager_PlayersSwapped;
        GamePlayerManager.PlayersAboutToSwap += GamePlayerManager_PlayersAboutToSwap;
    }

    private void OnDisable()
    {
        ControlsPanel.ControlsOpened -= OnMenuPanelOpened;
        ControlsPanel.ControlsClosed -= OnMenuPanelClosed;
        MovesListPanel.MovesListOpened -= OnMenuPanelOpened;
        MovesListPanel.MovesListClosed -= OnMenuPanelClosed;

        GamePlayerManager.PlayersSwapped -= GamePlayerManager_PlayersSwapped;
        GamePlayerManager.PlayersAboutToSwap -= GamePlayerManager_PlayersAboutToSwap;
    }

    private void GamePlayerManager_PlayersAboutToSwap(object sender, System.EventArgs e)
    {
        if (token != null)
        {
            DropToken();
        }
        gamePlayerSlot.ClearCharacter();
    }

    private void GamePlayerManager_PlayersSwapped(object sender, System.EventArgs e)
    {
        if (gamePlayerSlot != null)
        {
            // remove human from my slot before swapping to a new one, unless the other player already swapped into that one
            if (gamePlayerSlot.humanPlayerConfig == humanPlayerConfig)
            {
                gamePlayerSlot.SetHumanPlayerConfig(null);
            }
        }

        humanPlayerConfig.Input.uiInputModule = null;

        // human that controls this cursor won't change, but the slot (and player number we are) might
        SetUpGamePlayerSlot();
    }

    private void OnMenuPanelOpened(object sender, int e)
    {
        cursorEnabled = false;
    }
    private void OnMenuPanelClosed(object sender, int e)
    {
        cursorEnableDelay = cursorEnableDelayMax;
    }
    private void OnMenuPanelOpened(object sender, EventArgs e)
    {
        OnMenuPanelOpened(sender, -1);
    }
    private void OnMenuPanelClosed(object sender, EventArgs e)
    {
        OnMenuPanelClosed(sender, -1);
    }
}
