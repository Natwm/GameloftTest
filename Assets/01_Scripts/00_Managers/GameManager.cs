using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        RUNNING,
        MENU,
        PAUSE
    }


    public static GameManager instance;

    public Transform spawnerTransform;
    public GameState state = GameState.MENU;

    [SerializeField] private Transform _BallDirectionZone;

    private int amountOfElementToDestroy;

    private List<GameObject> allBall = new List<GameObject>();
    
    private float levelTimer;

    private bool isPaused = false;


    void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : GameManager");
        instance = this;
    }

    private void Start()
    {
        amountOfElementToDestroy = FindObjectsOfType<Target_ObstacleBehaviours>().Length;

        Vibration.Init();
    }

    public void Update()
    {
        if (state == GameState.RUNNING)
        {
            levelTimer -= Time.deltaTime;
            if (levelTimer <= 0)
                GameOver();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var item in FindObjectsOfType<Target_ObstacleBehaviours>())
            {
                item.Dead();
            }
        }
            

        CanvasManager.instance.gameTimer.text = GetLevelTimer();
    }

    /// <summary>
    /// Spawn the first ball and change the state of the game to RUNNING.
    /// </summary>
    /// <param name="ball"></param>
    public void SpawnFirstBall(GameObject ball)
    {
        GameObject firstBall = Instantiate(ball, spawnerTransform.position, Quaternion.identity);
        state = GameState.RUNNING;
        allBall.Add(firstBall);
    }

    /// <summary>
    /// Reduce the total of Block the player have to kill before he/she win the level
    /// </summary>
    public void ReduceAmountofElement()
    {
        amountOfElementToDestroy--;
        
        if (amountOfElementToDestroy <= 0)
            Victory();
    }

    /// <summary>
    /// SetUp the victory Panel
    /// </summary>
    public void Victory()
    {
        ScoreManager.instance.GetScore(Mathf.RoundToInt(levelTimer));
        DestroyAllBall();
        CanvasManager.instance.VictoryPanel();

        state = GameState.MENU;
    }

    public void GameOver()
    {
        print("GameOver");
        ScoreManager.instance.GetScore(Mathf.RoundToInt(levelTimer));
        DestroyAllBall();
        CanvasManager.instance.VictoryPanel();

        state = GameState.MENU;
    }

    /// <summary>
    /// Destroy all ball in the scene.
    /// </summary>
    public void DestroyAllBall()
    {
        foreach (var item in allBall)
        {
            Destroy(item);
        }

        allBall.Clear();
    }

    #region Timer

    /// <summary>
    /// Convert the float level Timer into a understanding timer with minutes and seconds
    /// </summary>
    /// <returns>  The timer in mm : ss (m = minutes| s = second) </returns>
    public string GetLevelTimer()
    {
        int minutes, seconde;
        if (levelTimer > 60)
        {
            minutes = (int)levelTimer / 60;
            seconde = (int)(levelTimer - (minutes * 60));
            return minutes + " : " + seconde;
        }
        else
            return Mathf.RoundToInt(levelTimer).ToString().Length > 1 ? "00 : " + Mathf.RoundToInt(levelTimer).ToString() : "00 : 0" + Mathf.RoundToInt(levelTimer).ToString();
    }

    public void IncreaseTimer(float amountOfTime)
    {
        levelTimer += amountOfTime;
        StartCoroutine(CanvasManager.instance.TimerFeedback(Mathf.RoundToInt(amountOfTime)));
    }
    #endregion

    public void PauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0.0f;
            state = GameState.PAUSE;
        }
        else
        {
            Time.timeScale = 1.0f;
            state = GameState.RUNNING;
        }
            
    }

    public void SetUpInitialLevelTimer(float time)
    {
        levelTimer = time;
    }

    #region GETTER && SETTER
    public Transform BallDirectionZone { get => _BallDirectionZone; set => _BallDirectionZone = value; }

    #endregion
}
