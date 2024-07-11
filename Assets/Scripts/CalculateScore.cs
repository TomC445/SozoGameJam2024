using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalculateScore : MonoBehaviour
{
    public TextMeshProUGUI traders;
    public TextMeshProUGUI towers;
    public TextMeshProUGUI enemies;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI final;
    public GameManager gameManager;
    public RoundTimer timerObj;

    public void SetScore(bool isDead)
    {
        //calculate score elements
        float multiplier = 1 + Mathf.Max((float)Mathf.Round((timerObj.targetTime) / timerObj.totalTimer * 100) / 100, 0);
        if (isDead) multiplier = 0.75f;
        float tradertotal = gameManager.traderShipNo - gameManager.traderShips.Count;
        float towertotal = gameManager.towersNo - gameManager.towers.Count;
        float enemytotal = gameManager.enemyShipNo - gameManager.enemyShips.Count;
        float finalScore = (tradertotal * 20 + towertotal * 50 + enemytotal * 100) * multiplier;

        //set text values based on score elements
        traders.text = $"Traders: {tradertotal} x 20pts";
        towers.text = $"Towers: {towertotal} x 50pts";
        enemies.text = $"Enemies: {enemytotal} x 100pts";
        timer.text = $"Time: {Mathf.Round(timerObj.actualTime)}s";
        multiplierText.text = $"Multiplier: {multiplier}";
        final.text = $"Score: {finalScore}";
    }
}
