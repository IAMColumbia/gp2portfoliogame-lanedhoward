using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player 1")]
    public FighterMain player1;
    public Healthbar player1Healthbar;

    [Header("Player 2")]
    public FighterMain player2;
    public Healthbar player2Healthbar;

    private void Update()
    {
        player1Healthbar.SetHealthbar(player1.CurrentHealth, player1.MaxHealth);
        player2Healthbar.SetHealthbar(player2.CurrentHealth, player2.MaxHealth);
    }
}
