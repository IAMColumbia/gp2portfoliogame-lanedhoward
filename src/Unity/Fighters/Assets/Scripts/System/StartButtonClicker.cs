using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartButtonClicker : MonoBehaviour
{
    public TextMeshProUGUI text;

    public float delay = 3f;

    public int count = 0;

    public int countMax = 3;

    private void Start()
    {
        text.CrossFadeColor(new Color(1, 1, 1, 0), 0, true, true);
    }

    public IEnumerator ResetCount()
    {
        yield return new WaitForSecondsRealtime(delay);

        count = 0;
    }

    /// <summary>
    /// returns if it is time to go to character select
    /// </summary>
    /// <returns></returns>
    public bool PressStart()
    {
        StopAllCoroutines();

        count++;

        if (count >= countMax)
        {
            return true;
        }

        int howManyMore = countMax - count;

        if (howManyMore == 1)
        {
            text.text = $"Press {howManyMore} more time to go back to menu...";
        }
        else
        {
            text.text = $"Press {howManyMore} more times to go back to menu...";
        }

        text.CrossFadeColor(new Color(1, 1, 1, 1), 0, true, true);

        text.CrossFadeColor(new Color(1, 1, 1, 0), delay, true, true);

        StartCoroutine(ResetCount());

        return false;
    }
}
