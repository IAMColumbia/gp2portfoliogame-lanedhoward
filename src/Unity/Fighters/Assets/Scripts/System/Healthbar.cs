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

    [SerializeField]
    private bool draining;

    public TextMeshProUGUI nametag;
    public TextMeshProUGUI descriptionTag;
    public Image headshot;

    public Image[] hearts;

    public SuperPortrait superPortrait;

    public GameObject burst;

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

    public virtual void SetHealthbar(float current, float max, bool setDrainBar = false)
    {
        if (current <= 0 || max <= 0)
        {
            percent = 0; 
        }
        else
        {
            percent = Mathf.Pow((current / max), gutsPower);
            percent = Mathf.Max(percent, minPercent);
        }

        if (drainbar != null)
        {
            if (setDrainBar || percent >= drainbar.fillAmount)
            {
                drainbar.fillAmount = percent;
            }
        }
    }

    public void SetCharacterModule(CharacterModule character)
    {
        nametag.SetText(character.CharacterName);
        descriptionTag.SetText(character.CharacterDescription);
        headshot.sprite = character.Headshot;
        superPortrait.portrait.sprite = character.Portrait;
    }

    public void SetMaterial(Material mat)
    {
        headshot.material = mat;
        foreach (var h in hearts)
        {
            h.material = mat;
        }

        superPortrait.portrait.material = mat;
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
        fighter.SuperPortraitStarted += superPortrait.StartSuperPortrait;
        fighter.SpentBurst += Fighter_SpentBurst;
    }

    private void Fighter_SpentBurst(object sender, System.EventArgs e)
    {
        burst.SetActive(false);
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
