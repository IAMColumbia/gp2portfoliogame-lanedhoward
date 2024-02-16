using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    PlayerInput playerInput;

    public float moveSpeed = 10f;

    public TextMeshPro PlayerText;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;

    public void SetGraphicRaycaster(GraphicRaycaster raycaster)
    {
        this.raycaster = raycaster;
    }

    public void SetPlayerInput(PlayerInput pi)
    {
        playerInput = pi;
        PlayerText.text = $"P{playerInput.playerIndex + 1}";
    }

    private void Awake()
    {
        pointerEventData = new PointerEventData(null);
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
                //pointerEventData.position = WorldToScreenSpace(transform.position, Camera.main, canvasTransform);//Camera.main.WorldToScreenPoint(transform.position);
                pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);

                //pointerEventData.position = Vector2.zero;
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

    public static Vector3 WorldToScreenSpace(Vector3 worldPos, Camera cam, RectTransform area)
    {
        Vector3 screenPoint = cam.WorldToScreenPoint(worldPos);
        screenPoint.z = 0;

        Vector2 screenPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, cam, out screenPos))
        {
            return screenPos;
        }

        return screenPoint;
    }
}
