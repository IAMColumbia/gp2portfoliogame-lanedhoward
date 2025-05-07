using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    public RawImage image;
    public Vector2 scrollSpeed;

    // Update is called once per frame
    void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + scrollSpeed * Time.deltaTime, image.uvRect.size);
    }
}
