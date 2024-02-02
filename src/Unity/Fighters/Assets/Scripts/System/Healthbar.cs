using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public float percent;
    public Image healthbar;
    public Image drainbar;
    public float minPercent;
    public float gutsPower;
    public float drainRate;

    private bool draining;

    public TextMeshProUGUI nametag;
    public Image portrait;

    public Image[] hearts;


    // Update is called once per frame
    void Update()
    {
        if (healthbar != null)
        {
            healthbar.fillAmount = percent;
            
            if (drainbar != null)
            {
                if (draining)
                {
                    if (drainbar.fillAmount > percent)
                    {
                        drainbar.fillAmount -= drainRate * Time.deltaTime;
                        if (drainbar.fillAmount < percent)
                        {
                            drainbar.fillAmount = percent;
                        }
                    }
                }
            }
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

    public void InitializeEvents(FighterMain fighter)
    {
        healthbar.fillAmount = 1;
        drainbar.fillAmount = 1;

        fighter.GotHit += Fighter_GotHit;
        fighter.LeftHitstun += Fighter_LeftHitstun;
    }

    private void Fighter_LeftHitstun(object sender, System.EventArgs e)
    {
        // might need a cooldown
        draining = true;
    }

    private void Fighter_GotHit(object sender, System.EventArgs e)
    {
        draining = false;
    }
}
