using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject traderShipsHolder;
    public GameObject enemyShipsHolder;
    public GameObject towersHolder;

    public int targetTime = 60;
    public bool regularPlay;

    public List<GameObject> traderShips = new List<GameObject>();
    public List<GameObject> enemyShips = new List<GameObject>();
    public List<GameObject> towers = new List<GameObject>();

    public int traderShipNo;
    public int enemyShipNo;
    public int towersNo;

    public Health playerHealth;

    public TextMeshProUGUI traderShipsText;
    public TextMeshProUGUI enemyShipsText;
    public TextMeshProUGUI towersText;

    public GameObject[] gameActive;
    public GameObject[] gameOver;
    public GameObject pauseMenu;

    public CalculateScore calcScore;

    private bool isGameOver;
    private GameConfig config;
    private bool isPaused = false;
    
    
    void Start()
    {
        isPaused = false;
        Time.timeScale = 1;
        isGameOver = false;
        towers = GetImmediateChildren(towersHolder);
        traderShips = GetImmediateChildren(traderShipsHolder);
        enemyShips = GetImmediateChildren(enemyShipsHolder);

        traderShipNo = traderShips.Count;
        enemyShipNo = enemyShips.Count;
        towersNo = towers.Count;

        config = GameObject.Find("Config").GetComponent<GameConfig>();
        targetTime = config.seconds;
        regularPlay = config.regularPlay;

        AudioListener.volume = (float)config.volume/100;
        UpdateScore();
    }

    void Update()
    {
        if (playerHealth.isDead)
        {
            GameOver();
        } else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPaused)
                {
                    Time.timeScale = 0;
                    pauseMenu.SetActive(true);
                    isPaused = true;
                } else
                {
                    Unpause();
                }
            }
        }
    }

    //unpause the game
    public void Unpause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    //get list of enemy types from enemy holders
    List<GameObject> GetImmediateChildren(GameObject parent)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
        }
        return children;
    }

    //update the score when an enemy is destroyed
    public void UpdateScore()
    {
        traderShipsText.text = $"x{traderShips.Count}";
        enemyShipsText.text = $"x{enemyShips.Count}";
        towersText.text = $"x{towers.Count}";
    }

    //game over procedure
    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = !isGameOver;
            foreach (GameObject active in gameActive)
            {
                active.SetActive(false);
            }
            foreach (GameObject over in gameOver)
            {
                over.SetActive(true);
            }
            calcScore.SetScore(playerHealth.isDead);
        }
    }

    //restart the game with the current settings
    public void RestartGame()
    {
        Unpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //load the main menu scene
    public void MainMenu()
    {
        Unpause();
        SceneManager.LoadScene("MainMenu");
    }

    //remove an enemy when killed
    public void RemoveEntity(GameObject entity)
    {
        EnemyType type = entity.gameObject.GetComponent<EnemyDeath>().enemyType;
        switch (type)
        {
            case EnemyType.Enemy:
                enemyShips.Remove(entity);
                break;
            case EnemyType.Tower:
                towers.Remove(entity);
                break;
            case EnemyType.Trader:
                traderShips.Remove(entity);
                break;
        }

        if(towers.Count == 0 && enemyShips.Count == 0 && traderShips.Count == 0)
        {
            GameOver();
        }

        UpdateScore();
    }
}
