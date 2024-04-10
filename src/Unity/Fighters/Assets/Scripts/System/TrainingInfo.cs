using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainingInfo : MonoBehaviour
{
    public TextMeshProUGUI text;

    public float maxDamage;

    public void Start()
    {
        maxDamage = 0f;
        SetInfo(new Combo());
    }

    public void SetInfo(Combo combo)
    {
        if (combo.totalDamage > maxDamage)
        {
            maxDamage = combo.totalDamage;
        }

        text.text = $@"Damage: {combo.totalDamage} (Max: {maxDamage})
Damage Scale: {combo.damageScale:F2}
Knockback Scale: {combo.knockbackScale}";
    }
}
