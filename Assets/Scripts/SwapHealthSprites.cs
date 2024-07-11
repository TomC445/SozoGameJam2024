using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapHealthSprites : MonoBehaviour
{
    private Health playerHealth;
    public Sprite[] playerSprites;
    public GameObject healthBarObj;
    private Image healthBarImg;

    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<Health>();
        healthBarImg = healthBarObj.GetComponent<Image>();
    }

    void Update()
    {
        healthBarImg.sprite = playerSprites[playerHealth.health];
    }
}
