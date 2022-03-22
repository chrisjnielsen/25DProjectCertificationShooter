using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject[] powerUps;
    [SerializeField]
    private int powerUpType, powerUpCount;
    [SerializeField]
    private bool spawnAllowed;
    

    // Start is called before the first frame update
    void Start()
    {
        powerUpCount = 0;
        if (Random.Range(0, 10) > 6)
        {
            powerUpCount++;
            spawnAllowed = true;
            transform.GetComponentInParent<Enemy>().PowerUpEnemy();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.position.x < 30f && this.gameObject.transform.position.x >29f && spawnAllowed == true)
        {
            if (powerUpCount == 1)
            {
                powerUpType = Random.Range(0, 4);
                GameObject go = Instantiate(powerUps[powerUpType], this.transform.position, Quaternion.identity);
                go.transform.parent = null;
                spawnAllowed = false;
            }
        }
        if (this.gameObject.transform.position.x > 107) spawnAllowed = true;

    }

    IEnumerator PowerUp()
    {
        powerUpType = Random.Range(0, 4);
        GameObject go = Instantiate(powerUps[powerUpType], this.transform.position, Quaternion.identity);
        go.transform.parent = null;
        yield return new WaitForSeconds(6f);
    }

    
}
