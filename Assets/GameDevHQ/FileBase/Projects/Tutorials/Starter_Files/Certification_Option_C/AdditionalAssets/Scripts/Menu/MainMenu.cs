using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _optionsMenu;
    private string _checkpointValue;
    [SerializeField]
    private GameObject _continueButton;
    private CheckpointMaster _checkPointStatus;
    [SerializeField]
    private TextMeshProUGUI highScore;
    private int newScore, prevScore;


    private void Awake()
    {
        
        _continueButton = GameObject.FindGameObjectWithTag("Continue");
        _continueButton.gameObject.SetActive(false);
        
        
    }

    private void OnEnable()
    {
        HighScore();
        if (GameObject.FindGameObjectWithTag("Checkpoint") != null) 
        {
            _checkPointStatus = GameObject.FindGameObjectWithTag("Checkpoint").GetComponent<CheckpointMaster>();
            if (_checkPointStatus.checkpoint == true)
            {
                _continueButton.gameObject.SetActive(true);
            }
        }
        else
        {
            //PlayerPrefs.DeleteAll();
            _continueButton.gameObject.SetActive(false);
        }
        
        
        
    }

    public void HighScore()
    {
        newScore = PlayerPrefs.GetInt("playerscore");
        if (prevScore > newScore)
        {
            newScore = prevScore;
        }
        highScore.text = "HIGH SCORE: " + newScore+ " COINS";
    }

    public void LoadGame()
    {
        if (GameObject.FindGameObjectWithTag("Checkpoint") != null) _checkPointStatus.NewGame();
        //PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadOptions()
    {
        _optionsMenu.SetActive(true);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void CloseOptions()
    {
        _optionsMenu.SetActive(false);
    }
}
