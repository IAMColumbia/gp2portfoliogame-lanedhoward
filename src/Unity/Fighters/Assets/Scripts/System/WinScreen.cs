using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public TextMeshProUGUI p1wins;
    public TextMeshProUGUI p2wins;

    public void UpdateRecord(int _p1wins, int _p2wins)
    {
        p1wins.text = _p1wins.ToString();
        p2wins.text = _p2wins.ToString();
    }
}
