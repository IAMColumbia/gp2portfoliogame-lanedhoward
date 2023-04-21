using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboUI : MonoBehaviour
{
    public TextMeshProUGUI comboCountText;
    public TextMeshProUGUI comboDamageText;

    public Color32 activeColor;
    public Color32 exitColor;

    public float comboTimeout = 1.5f;

    public Coroutine endCombocount;

    public void UpdateComboText(int ComboCount, float TotalDamage)
    {
        if (endCombocount != null)
        {
            StopCoroutine(endCombocount);
        }
        //comboCountText.color = activeColor;
        //comboDamageText.color = activeColor;
        comboCountText.CrossFadeColor(activeColor, 0f, false, false);
        comboDamageText.CrossFadeColor(activeColor, 0f, false, false);
        comboCountText.text = ComboCount + " hit" + ((ComboCount > 1) ? "s" : "") + "!!!";
        comboDamageText.text = TotalDamage + " damage";
    }

    public void ShowText()
    {
        this.gameObject.SetActive(true);
    }

    public void HideText()
    {
        this.gameObject.SetActive(false);
    }

    public void StartEndComboCount()
    {
        endCombocount = StartCoroutine(EndComboCount());
    }

    public IEnumerator EndComboCount()
    {
        //comboCountText.color = exitColor;
        //comboDamageText.color = exitColor;
        comboCountText.CrossFadeColor(exitColor, 0.25f, false, false);
        comboDamageText.CrossFadeColor(exitColor, 0.25f, false, false);
        yield return new WaitForSeconds(comboTimeout);
        HideText();
    }
}
