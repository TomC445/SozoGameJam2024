using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundTimer : MonoBehaviour
{
    public float targetTime = 60.0f;
    public float actualTime = 0f;
    public float totalTimer;
    public TextMeshProUGUI timerText;
    public GameManager gameManager;

    void Start()
    {
        totalTimer = targetTime;
        targetTime = gameManager.targetTime;
    }

    void Update()
    {
        if (gameManager.regularPlay)
        {
            NormalPlay();
        } else
        {
            CountDown();
        }
    }

    void NormalPlay()
    {
        targetTime = 0;
        actualTime += Time.deltaTime;
        timerText.text = Mathf.Round(actualTime).ToString();
    }

    void CountDown()
    {
        targetTime -= Time.deltaTime;
        actualTime += Time.deltaTime;
        timerText.text = Mathf.Round(targetTime).ToString();
        if (targetTime <= 0.0f)
        {
            gameManager.GameOver();
        }
    }
}
