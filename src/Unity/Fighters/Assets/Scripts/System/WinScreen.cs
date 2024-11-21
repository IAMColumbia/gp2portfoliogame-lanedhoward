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

    public void UpdateRecord(int _p1wins, int _p2wins)
    {
        p1wins.text = _p1wins.ToString();
        p2wins.text = _p2wins.ToString();
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
    }

    public void HideWinScreen()
    {
        record.SetActive(false);
        foreach (PostGameUIPanels pg in postGameUIs)
        {
            pg.Hide();
            pg.rematchMessage.SetActive(false);
        }
    }
}
