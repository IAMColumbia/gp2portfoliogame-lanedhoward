using System.Collections;
using System.Collections.Generic;
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
        //Hopefully the GamePlayerManager should know about the PI by now, so we should be able to set our HumanPlayerConfig
        GamePlayerManager.Instance.SetUpCursor(this);
        uiManager.SetUpCursor(this);

        PlayerText.text = $"P{gamePlayerSlot.PlayerSlotIndex + 1}";
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
                transform.position += move * moveSpeed * Time.deltaTime;
            }


            if (playerInput.actions["AttackA"].WasPressedThisFrame())
            {
                pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);

                List<RaycastResult> results = new();
                raycaster.Raycast(pointerEventData, results);

                if (results.Count > 0)
                {
                    foreach (var r in results)
                    {
                        CursorButton button = r.gameObject.GetComponent<CursorButton>();
                        if (button != null) button.Interact(this);
                    }
                }
            }

        }
    }
}
