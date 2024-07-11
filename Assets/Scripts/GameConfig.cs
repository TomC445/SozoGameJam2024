using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameConfig : MonoBehaviour
{
    public int seconds;
    public bool regularPlay;
    public float volume = 25;
    public float backgroundMusicVolume = 25;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Config");
        if(objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
