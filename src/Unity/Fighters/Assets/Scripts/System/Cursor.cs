using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    PlayerInput playerInput;

    public float moveSpeed = 10f;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;

    private RectTransform canvasTransform;

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
        //raycaster = GetComponentInParent<GraphicRaycaster>();
        //canvasTransform = transform.parent.GetComponent<RectTransform>();
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

            //pointerEventData.position = WorldToScreenSpace(transform.position, Camera.main, canvasTransform);//Camera.main.WorldToScreenPoint(transform.position);
            pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);

            //pointerEventData.position = Vector2.zero;
            List<RaycastResult> results = new();
            raycaster.Raycast(pointerEventData, results);

            if (results.Count > 0)
            {
                Debug.Log(results[0].gameObject.name);
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
