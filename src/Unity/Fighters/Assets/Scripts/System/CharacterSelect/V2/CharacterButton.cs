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
    //public CharacterModule[] allCharacters;
    
    void Awake()
    {
        //if (IsRandom)
        //{
        //    nameTag.text = "Random";
        //}
        //else
        //{
            if (character != null)
            {
                image.material = character.materials[0];
                nameTag.text = character.CharacterName;
            }
        //}
    }

    public void PressCharacterButton(Cursor cursor)
    {
        if (cursor.token == null) return;

        CharacterModule c = character;

        if (IsRandom)
        {
            //c = LaneLibrary.RandomMethods.Choose(allCharacters);
            cursor.token.slot.gamePlayerConfig.IsRandomSelect = true;
        }
        else
        {
            cursor.token.slot.gamePlayerConfig.IsRandomSelect = false;

        }

        cursor.token.slot.SetCharacter(c);

    }


}
