using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    private TextMeshProUGUI text;
    private float timer;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (timer < 0)
        {
            timer = -1;
            DisableText();
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public void EnableText(string newText, float time)
    {
        transform.SetAsFirstSibling();
        timer = time;
        text.text = newText;
        gameObject.SetActive(true);
    }

    public void DisableText()
    {
        //transform.SetAsLastSibling();
        gameObject.SetActive(false);
    }
}
