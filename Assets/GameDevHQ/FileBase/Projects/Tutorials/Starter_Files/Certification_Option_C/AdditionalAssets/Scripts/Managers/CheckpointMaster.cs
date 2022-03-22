using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointMaster : MonoBehaviour
{
    private static CheckpointMaster instance;
    public bool checkpoint = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else Destroy(this);
    }

    public void CheckpointReached()
    {
        checkpoint = true;
        PlayerPrefs.SetString("checkpoint", "true");
    }

    public void NewGame()
    {
        checkpoint = false;
    }
   
}
