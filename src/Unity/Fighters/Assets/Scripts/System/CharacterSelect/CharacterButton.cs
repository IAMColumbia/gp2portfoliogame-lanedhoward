using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CharacterButton : MonoBehaviour
{
    public CharacterModule character;
    public TextMeshProUGUI nameTag;
    public Image image;

    public bool IsRandom;
    public CharacterModule[] allCharacters;
    
    void Awake()
    {
        if (IsRandom)
        {
            nameTag.text = "Random";
        }
        else
        {
            if (character != null)
            {
                image.material = character.materials[0];
                nameTag.text = character.CharacterName;
            }
        }
    }

}
