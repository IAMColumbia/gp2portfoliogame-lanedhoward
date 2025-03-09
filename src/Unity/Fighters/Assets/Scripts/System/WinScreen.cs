using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public GameObject record;
    public PostGameUIPanels[] postGameUIs;

    public TextMeshProUGUI p1wins;
    public TextMeshProUGUI p2wins;

    public TextMeshProUGUI winQuote;


    public void UpdateRecord(int _p1wins, int _p2wins)
    {
        p1wins.text = _p1wins.ToString();
        p2wins.text = _p2wins.ToString();
    }

    public void SetWinQuoteText(string text)
    {
        winQuote.text = $"\" {text}\"";
    }

    public void ShowWinScreen()
    {
        record.SetActive(true);
        foreach (PostGameUIPanels pg in postGameUIs)
        {
            if (pg.UIActive)
            {
                pg.Show();
                pg.SetRematchSelected();
            }
            else
            {
                pg.Hide();
            }
        }
        winQuote.gameObject.SetActive(true);
        //Tween.Delay(0.1f).Chain(Tween.Custom(onValueChange: (float v) => winQuote.alpha = v, startValue: 0, endValue: 1, duration: 2));
        winQuote.CrossFadeAlpha(0f, 0f, false);
        winQuote.CrossFadeAlpha(1f, 3f, false);

    }

    public void HideWinScreen()
    {
        record.SetActive(false);
        foreach (PostGameUIPanels pg in postGameUIs)
        {
            pg.Hide();
            pg.rematchMessage.SetActive(false);
        }
        winQuote.gameObject.SetActive(false);
    }
}
