using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StocksDisplay : MonoBehaviour
{
    public TextMeshProUGUI stocksText;
    public Image stocksIcon;

    public void InitializeStocksDisplay(FighterMain fighter)
    {
        stocksText.gameObject.SetActive(fighter.displayStocks);
        stocksIcon.gameObject.SetActive(fighter.displayStocks);

        if (fighter.characterModule.stocksIcon != null)
        {
            SetIcon(fighter.characterModule.stocksIcon);
        }

        SetStocksText(fighter.GetStocks());

        fighter.StocksUpdated += Fighter_StocksUpdated;
    }

    private void Fighter_StocksUpdated(object sender, int e)
    {
        SetStocksText(e);
    }

    public void SetIcon(Sprite icon)
    {
        stocksIcon.sprite = icon;
    }

    public void SetStocksText(int stocks)
    {
        stocksText.SetText(stocks.ToString());
    }

    public void SetMaterial(Material mat)
    {
        stocksIcon.material = mat;
    }

}
