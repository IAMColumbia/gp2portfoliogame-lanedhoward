using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public float percent;
    public Image healthbar;

    // Update is called once per frame
    void Update()
    {
        if (healthbar != null)
        {
            healthbar.fillAmount = percent;
        }
    }

    public void SetHealthbar(float current, float max)
    {
        if (current <= 0 || max <= 0)
        {
            percent = 0; 
            return;
        }
        percent = current / max;
    }
}