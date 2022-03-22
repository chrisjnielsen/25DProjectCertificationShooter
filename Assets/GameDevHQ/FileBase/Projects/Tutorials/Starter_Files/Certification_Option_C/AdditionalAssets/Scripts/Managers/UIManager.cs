using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Images")]
    [SerializeField]
    private Image _weaponImg;
    [SerializeField]
    private Image _thrustIndicator;
    [SerializeField]
    private Sprite[] _weaponLevelImg;
    [SerializeField]
    private Image _missileDisplay;
    [SerializeField]
    private Sprite[] _missileSprites;

    [Header("UI Text")]
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private TextMeshProUGUI _gameOverText;
    [SerializeField]
    private TextMeshProUGUI _ammoText;
    [SerializeField]
    private TextMeshProUGUI _restartText;
    [SerializeField]
    private TextMeshProUGUI _cooldownText;
    [SerializeField]
    private TextMeshProUGUI _waveText;
    [SerializeField]
    private TextMeshProUGUI _enemyText;
    [SerializeField]
    private TextMeshProUGUI _waveScreenText;
    [SerializeField]
    private TextMeshProUGUI _missileFireText;
    [SerializeField]
    private TextMeshProUGUI _winText;
    [SerializeField]
    private TextMeshProUGUI _shieldText;
    [SerializeField]
    private GameObject _pausePanel;
    [SerializeField]
    private TextMeshProUGUI _checkPointText;


    [Header("Variables")]
    [SerializeField]
    private float _thrustTimer = 1f;
    [SerializeField]
    private float _maxThrusterTimer = 1f;
    [SerializeField]
    private KeyCode _selectKey = KeyCode.LeftShift;

    private int wavecurrent =0;
    private int wavetotal = 0;
   


    private bool _shouldUpdateThruster = false;

    private Player player;
    private bool _check;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Checkpoint") != null)
        {
            _check = GameObject.FindGameObjectWithTag("Checkpoint").GetComponent<CheckpointMaster>().checkpoint;
            
        }
        

    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("player is null");
        }
        //_cooldownText.enabled = false;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _winText.gameObject.SetActive(false);
        _pausePanel.gameObject.SetActive(false);
        ShowScoreOnContinue();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateWaveScreenText()
    {
        if (wavecurrent < wavetotal) StartCoroutine(ShowWaveText(wavecurrent));
        if (wavecurrent == wavetotal) StartCoroutine(ShowWaveCompleteText());
    }

    IEnumerator ShowWaveText(int currentwave)
    {
        _waveScreenText.gameObject.SetActive(true);
        //if (_check == true) PlayerPrefs.SetInt("playerwave", currentwave);
        _waveScreenText.text = "Wave " + currentwave + " Complete! Next Wave...";
        yield return new WaitForSeconds(2f);
        _waveScreenText.gameObject.SetActive(false);

    }

    IEnumerator ShowWaveCompleteText()
    {
        _waveScreenText.gameObject.SetActive(true);
        _waveScreenText.text = "All Waves Completed! Good Job!";
        yield return null;
    }

    public void CheckPointReached()
    {
        StartCoroutine(ShowCheckPointText());
    }

    IEnumerator ShowCheckPointText()
    {
        _checkPointText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        _checkPointText.gameObject.SetActive(false);
    }
    
    public void ShowScoreOnContinue()
    {
        
        if (_check == true)
        {
            
            _scoreText.text = "Score: " + PlayerPrefs.GetInt("playerscore") + " COINS";
        }

    }
    public void UpdateEnemyCount()
    {

        _enemyText.text = "Enemies: " + (GameManager.Instance.CurrentEnemyCount);
        if (GameManager.Instance.CurrentEnemyCount == 0)
        {
            UpdateWaveScreenText();
        }
    }

    public void UpdateShield(int shieldCount)
    {
        _shieldText.text = "Shields: " + shieldCount + "/5";
    }

    public void UpdateWaves(int waveCurrent, int waveTotal)
    {
        wavecurrent = waveCurrent;
        wavetotal = waveTotal;
        _waveText.text = "Wave:  " + waveCurrent + "  /  " + waveTotal;
        if(_check == true) GameManager.Instance.SaveWaveStatus(wavecurrent, wavetotal);  
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score + " COINS";

        if (score > PlayerPrefs.GetInt("playerscore")) GameManager.Instance.SaveScoreStatus(score);
    }
    public void UpdateAmmo(int ammoCurrent, int ammoTotal)
    {
        _ammoText.text = "Ammo:  " + ammoCurrent + "  /  " + ammoTotal;
    }

    public void UpdateThruster()
    {

        //Show radial dial for thruster in use, hide when not in use and also hide when use is exceeded and need to cool down
        //Show cooldown text when thruster timer used up, or just holding in thruster key, reset when thruster key lifted
        if (Input.GetKey(_selectKey))
        {
            //player.ThrusterOn();
            _cooldownText.gameObject.SetActive(false);
            _shouldUpdateThruster = false;
            _thrustTimer -= Time.deltaTime;
            _thrustIndicator.enabled = true;
            _thrustIndicator.fillAmount = _thrustTimer;

            if (_thrustTimer <= 0)
            {
                //player.ThrusterOff();
                _thrustTimer = 0;
                _thrustIndicator.fillAmount = 0;
                _thrustIndicator.enabled = false;
                _cooldownText.gameObject.SetActive(true);
            }
        }
        else
        {
            if (_shouldUpdateThruster)
            {
                _cooldownText.gameObject.SetActive(false);
                //player.ThrusterOn();
                _thrustTimer += Time.deltaTime;
                _thrustIndicator.fillAmount = _thrustTimer;
                if (_thrustTimer >= _maxThrusterTimer)
                {
                    //player.ThrusterOff();
                    _cooldownText.gameObject.SetActive(false);
                    _thrustIndicator.enabled = false;
                    _shouldUpdateThruster = false;
                }
            }
        }

        if (Input.GetKeyUp(_selectKey))
        {
            _shouldUpdateThruster = true;
        }
    }


    public void UpdateMissiles(int currentMissiles)
    {
        if (currentMissiles > 6) currentMissiles = 6;
        _missileDisplay.sprite = _missileSprites[currentMissiles];
        if (currentMissiles > 0) _missileFireText.gameObject.SetActive(true);
        if (currentMissiles == 0) _missileFireText.gameObject.SetActive(false);
    }


    public void UpdateWeaponLevel(int weaponLevel)
    {
        //make sure max lives cannot go above 3
        if (weaponLevel > 3) weaponLevel = 3;
        _weaponImg.sprite= _weaponLevelImg[weaponLevel];
        if (weaponLevel < 0)
        {
            weaponLevel = 0;
            GameOverSequence();
        }
    }
    public void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
        GameManager.Instance.GameOver();
    }


    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER!";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator GameWinFlicker()
    {
        while (true)
        {
            _winText.text = "WINNER! ALL STAGES CLEARED \n THANK YOU FOR PLAYING!";
            yield return new WaitForSeconds(0.5f);
            _winText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void PauseMenuShow()
    {
        _pausePanel.gameObject.SetActive(true);

    }

    public void PauseMenuHide()
    {
        _pausePanel.gameObject.SetActive(false);
    }

    public void RestartSelect()
    {
        GameManager.Instance.RestartGame();
    }


    public void ContinueSelect()
    {
        GameManager.Instance.ContinueGame();
    }

    public void GameWin()
    {
        _winText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameWinFlicker());
        GameManager.Instance.GameOver();
    }

}
