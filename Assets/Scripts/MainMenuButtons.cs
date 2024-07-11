using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuButtons : MonoBehaviour
{
    public GameConfig gameConfig;
    public Slider volumeSlider;
    public TextMeshProUGUI volumeText;
    public Slider bgvolumeSlider;
    public TextMeshProUGUI bgvolumeText;

    private void Start()
    {
        volumeSlider.value = gameConfig.volume;
        volumeText.text = $"{gameConfig.volume}/100";
        bgvolumeSlider.value = gameConfig.backgroundMusicVolume;
        bgvolumeText.text = $"{gameConfig.backgroundMusicVolume}/100";
    }

    public void PlayGame()
    {
        gameConfig.regularPlay = true;
        StartGame();
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void StartTimeAttack(int seconds)
    {
        gameConfig.regularPlay = false;
        gameConfig.seconds = seconds;
        StartGame();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void UpdateSlider()
    {
        gameConfig.volume = volumeSlider.value;
        volumeText.text = $"{gameConfig.volume}/100";
        AudioListener.volume = (float)gameConfig.volume / 100;
    }

    public void UpdateBGSlider()
    {
        gameConfig.backgroundMusicVolume = bgvolumeSlider.value;
        bgvolumeText.text = $"{gameConfig.backgroundMusicVolume}/100";
    }

}
