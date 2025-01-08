using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpinner : MonoBehaviour
{
    [SerializeField]
    float spinRate;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, spinRate * Time.unscaledDeltaTime);
    }
}
