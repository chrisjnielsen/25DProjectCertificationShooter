using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScriptObj : MonoBehaviour
{
    //SpawnScriptObj may be accessed globally to send out the spawn waves
    static SpawnScriptObj _instance;
    public static SpawnScriptObj Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(SpawnScriptObj)) as SpawnScriptObj;
            }
            return _instance;
        }
        set { _instance = value; }
    }

    [SerializeField]
    private List<Wave> _waves = new List<Wave>();

    SpawnManager _spawnManager;
    public int _currentWave;

    [SerializeField]
    private float yMax = 9f;
    [SerializeField]
    private float yMin = -9f;
    [SerializeField]
    private GameObject _enemyContainer;
    private UIManager _uiManager;

    public static int enemyCount, count;

    private bool _stopSpawning = false;

    private bool _checkpointpassed = false;
    private CheckpointMaster _check;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _spawnManager = GameObject.FindGameObjectWithTag("Spawn").GetComponent<SpawnManager>();
       
        _uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        _check = GameObject.FindGameObjectWithTag("Checkpoint").GetComponent<CheckpointMaster>();
        if (_check.checkpoint == true)
        {
            _currentWave = PlayerPrefs.GetInt("playerwave");
            
            
            _uiManager.UpdateScore(PlayerPrefs.GetInt("playerscore"));
        }
        else _currentWave = 0;
        count = 0;
    }

    void Update()
    {

    }

    public void StartEnemySpawnWaves()
    {
        StartCoroutine(StartWaveRoutine());
    }

    IEnumerator StartWaveRoutine()
    {
        yield return new WaitForSeconds(2f);
        if (_currentWave == _waves.Count)
        {
            PlayerWin();
        }

        else if (_stopSpawning == true) yield return null;
        else
        {
            
            var currentWave = _waves[_currentWave].sequence;
          
            if (_checkpointpassed == true && count ==1)
            {
               
                PlayerPrefs.SetInt("playerwave", _currentWave);
                count++;
            }
                enemyCount = currentWave.Count;
                GameManager.Instance.CurrentEnemyCount = enemyCount;    //assign total enemy count from spawn wave, then subtract as each enemy destroyed
                _uiManager.UpdateWaves(_currentWave + 1, _waves.Count);
                _uiManager.UpdateEnemyCount();

                foreach (var obj in currentWave)
                {
                    Vector3 posToSpawn = new Vector3(49f, Random.Range(yMin, yMax), 0);
                    GameObject newEnemy = Instantiate(obj, posToSpawn, Quaternion.Euler(new Vector3(0, -90, 0)));
                    newEnemy.transform.parent = _enemyContainer.transform;
                    float _pauseSpawn = Random.Range(2f, 11f);
                    yield return new WaitForSeconds(_pauseSpawn);
                }
            

           
        }
    }

    public void Checkpoint()
    {
        _checkpointpassed = true;
        count = 1;

    }

    public void PlayerWin()
    {
        _stopSpawning = true;
        _uiManager.GameWin();
        _spawnManager.StopSpawn();
        _check.checkpoint = false;
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
        StopAllCoroutines();
        _spawnManager.StopSpawn();
    }
}
