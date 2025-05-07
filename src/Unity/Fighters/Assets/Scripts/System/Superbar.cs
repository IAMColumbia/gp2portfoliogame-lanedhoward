using JimmysUnityUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

public class Superbar : Healthbar
{
    public TextMeshProUGUI barNumberText;
    public float barAmount = 100;
    public int bars;


    public override void SetHealthbar(float current, float max, bool setDrainBar = false)
    {
        int newBars = (current / barAmount).FloorToInt();
        float overflow = current % barAmount;

        if (newBars != bars)
        {
            // dont drain if we are increasing to a new bar level
            setDrainBar = true;
        
            if (newBars < bars)
            {
                // going down bars, manually set drainbar to 100%
                if (drainbar != null)
                {
                    setDrainBar = false;
                    drainbar.fillAmount = 1;
                    
                }
            }
        }

        bars = newBars;

        barNumberText.text = bars.ToString();

        if (current == max) overflow = barAmount; // stay full at max meter

        base.SetHealthbar(overflow, barAmount, setDrainBar);
    }
}
