using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    private Player player;
    [SerializeField]
    private int speed = 15;
   

    private void Start()
    {
        

        StartCoroutine(WaitTime());
    }
    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(15f);
        Destroy(this.gameObject);
    }

    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.GetComponent<Player>();
            switch (this.gameObject.name)
            {

                case string a when a.Contains("Upgrade"):
                    AudioPlayer.Instance.playWeaponUpSound();
                    player.WeaponUp();
                    break;
                case string a when a.Contains("Coin"):
                    AudioPlayer.Instance.playCoinSound();
                    player.AddScore(100);
                    break;
                case string a when a.Contains("Stack"):
                    AudioPlayer.Instance.playCoinSound();
                    player.AddScore(500);
                    break;
                case string a when a.Contains("Shield"):
                    AudioPlayer.Instance.playShieldSound();
                    player.PlayerShield();
                    break;
                default:
                    return;
            }
            Destroy(this.gameObject);
        }
        else if (other.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
        
    }


  


}
