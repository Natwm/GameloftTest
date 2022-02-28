using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public LevelParametter_SO parametter;

    public int playerScore;

    void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : ScoreManager");
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.SetUpInitialLevelTimer(parametter.InitialTimer.GetParametterInSecond());
    }

    public void GetScore(int time)
    {
        playerScore = parametter.GetScore(time);
    }
}
