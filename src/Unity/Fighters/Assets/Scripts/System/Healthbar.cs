using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public float percent;
    public Image healthbar;
    public float minPercent;
    public float gutsPower;

    public TextMeshProUGUI nametag;
    public Image portrait;

    public Image[] hearts;

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
        percent = Mathf.Pow((current / max), gutsPower);
        percent = Mathf.Max(percent, minPercent);
    }

    public void SetNametag(string name)
    {
        nametag.SetText(name);
    }

    public void SetMaterial(Material mat)
    {
        //healthbar.CrossFadeColor(mat.GetColor("_MainColor"),0,false,false);
        //healthbar.material = mat;
        portrait.material = mat;
        foreach (var h in hearts)
        {
            h.material = mat;
        }
    }

    public void UpdateHearts(int lives)
    {
        foreach(var h in hearts)
        {
            h.gameObject.SetActive(true);
        }
        if (lives < 2)
        {
            hearts[1].gameObject.SetActive(false);
        }
        if (lives < 1)
        {
            hearts[0].gameObject.SetActive(false);
        }
    }
}
