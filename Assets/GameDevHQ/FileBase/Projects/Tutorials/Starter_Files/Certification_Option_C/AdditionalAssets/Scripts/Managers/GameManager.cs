using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    private PlayerActions playerInput;
    [SerializeField]
    private int waypointsFound = 0;
    private int midBossWayPointsFound = 0;
    public int finalBossWayPointsFound = 0;

    List<GameObject> _wayPoints = new List<GameObject>();
    List<GameObject> _midBossWayPoints = new List<GameObject>();
    List<GameObject> _bossWayPoints = new List<GameObject>();
    public List<GameObject> Waypoints
    {
        get
        {
            if (waypointsFound == 0)
            {
                //find Waypoints to assign to Enemy that are instantiated prefabs
                GameObject gameObjects = GameObject.FindGameObjectWithTag("Waypoint");
                waypointsFound++;
                foreach (Transform child in gameObjects.transform)
                {
                    _wayPoints.Add(child.gameObject);
                }
            }
            return _wayPoints;
        }
        set
        {
            _wayPoints = value;
        }
    }

    public List<GameObject> Waypoints2
    {
        get
        {
            if (midBossWayPointsFound == 0)
            {
                
                //find Waypoints to assign to Enemy that are instantiated prefabs
                GameObject midBossGameObjects = GameObject.FindGameObjectWithTag("Waypoint2");
                midBossWayPointsFound++;
                foreach (Transform child in midBossGameObjects.transform)
                {
                    _midBossWayPoints.Add(child.gameObject);
                }
            }
            return _midBossWayPoints;
        }
        set
        {
            _midBossWayPoints = value;
        }
    }


    public List<GameObject> BossWaypoints 
    { 
        get 
        {
            if (finalBossWayPointsFound == 0)
            {

                //find Waypoints to assign to Enemy that are instantiated prefabs
                GameObject finalBossGameObjects = GameObject.FindGameObjectWithTag("Waypoint3");
                finalBossWayPointsFound++;
                foreach (Transform child in finalBossGameObjects.transform)
                {
                    _bossWayPoints.Add(child.gameObject);
                }
            }
            return _bossWayPoints;
        }
        set
        {
            _bossWayPoints = value;
        }
    }

    public List<GameObject> BossWaypoints2ndStage
    {
        get
        {
            if (finalBossWayPointsFound == 0)
            {

                //find Waypoints to assign to Enemy that are instantiated prefabs
                GameObject finalBossGameObjects = GameObject.FindGameObjectWithTag("Waypoint4");
                finalBossWayPointsFound++;
                foreach (Transform child in finalBossGameObjects.transform)
                {
                    _bossWayPoints.Add(child.gameObject);
                }
            }
            return _bossWayPoints;
        }
        set
        {
            _bossWayPoints = value;
        }
    }


    public List<GameObject> BossWaypointsFinalStage
    {
        get
        {
            if (finalBossWayPointsFound == 0)
            {

                //find Waypoints to assign to Enemy that are instantiated prefabs
                GameObject finalBossGameObjects = GameObject.FindGameObjectWithTag("Waypoint5");
                finalBossWayPointsFound++;
                foreach (Transform child in finalBossGameObjects.transform)
                {
                    _bossWayPoints.Add(child.gameObject);
                }
            }
            return _bossWayPoints;
        }
        set
        {
            _bossWayPoints = value;
        }
    }
    //Game Manager can be accessed globally, holds references to waypoints, to then assign to Enemy

    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }
            return _instance;
        }
        set { _instance = value; }
    }

    int currentEnemyCount;
    // Getter Setter for tracking enemy count and advancing the spawn waves
    public int CurrentEnemyCount
    {
        get { return currentEnemyCount; }
        set
        {
            currentEnemyCount = value;
            if (currentEnemyCount <= 0)
            {
                SpawnScriptObj.Instance._currentWave++;
                SpawnScriptObj.Instance.StartEnemySpawnWaves();

            }
        }
    }

    private UIManager _uiManager;
    bool gameIsPaused;

    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this;
        }
        playerInput = new PlayerActions();
    }

    private void OnEnable()
    {
        playerInput.Enable();
        if (_uiManager == null)
        {
            _uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        }
    }

    private void OnDisable()
    {
        if (playerInput != null) playerInput.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //if escape key quit application

        if (playerInput.Keyboard.Escape.triggered)
        {
            SceneManager.LoadScene(0);
        }

        if (playerInput.Keyboard.Restart.triggered && _isGameOver == true)
        {
            SceneManager.LoadScene(1);
        }

        if (playerInput.Keyboard.Pause.triggered)
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
            if (gameIsPaused == true) _uiManager.PauseMenuShow();
            else _uiManager.PauseMenuHide();
        }


    }

    void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
        }
    }

    public void RestartGame()
    {
        gameIsPaused = false;
        PauseGame();
        GameOver();
        _uiManager.PauseMenuHide();
        SceneManager.LoadScene(1);

    }

    public void ContinueGame()
    {
        gameIsPaused = false;
        PauseGame();
        _uiManager.PauseMenuHide();

    }

    public void SaveScoreStatus(int scoreValue)
    {
        PlayerPrefs.SetInt("playerscore", scoreValue);
    }

    public void SaveWaveStatus(int waveNumber, int waveTotal)
    {
        //PlayerPrefs.SetInt("playerwave", waveNumber);
        //PlayerPrefs.SetInt("totalwave", waveTotal);
    }

    public void MidBossBeaten()
    {
        CheckpointMaster checkpoint = GameObject.FindGameObjectWithTag("Checkpoint").GetComponent<CheckpointMaster>();
        checkpoint.CheckpointReached();
        _uiManager.CheckPointReached();
        SpawnScriptObj.Instance.Checkpoint();
    }


    public void GameOver()
    {
        _isGameOver = true;
        _wayPoints.Clear();
        _midBossWayPoints.Clear();
        _bossWayPoints.Clear();
        midBossWayPointsFound = 0;
        waypointsFound = 0;
        finalBossWayPointsFound = 0;
    }
}
