using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{ 
    public bool stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        this.GetComponent<SpawnScriptObj>().StartEnemySpawnWaves();
        //different enemy coroutine
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void StopSpawn()
    {
        stopSpawning = true;
    }
}
