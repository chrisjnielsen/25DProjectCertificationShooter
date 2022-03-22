using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDiagUp : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private int _diffFactor;

    public bool _enemyWeapon = false;

    private void Start()
    {
        _diffFactor = PlayerPrefs.GetInt("Difficulty");
        if (_enemyWeapon == true)
        {
            switch (_diffFactor)
            {
                case (0):
                    _speed = 20f;
                    break;
                case (1):
                    _speed = 35f;
                    break;
                case (2):
                    _speed = 45f;
                    break;
                default:
                    break;
            }
        }
        else _speed = 20f;
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.tag == "Enemy")
        {
            if (_enemyWeapon == true) return;
            else
            {


            }
        }
        else if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            player.Damage();
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.layer == 9)
        {
            EnemyMoveDiagUp();
        }
        else MoveDiagUp();
    }

    void MoveDiagUp()
    {
        _enemyWeapon = false;
        transform.Translate(new Vector3(1, 0.25f, 0) * _speed * Time.deltaTime, Space.World);
        if (transform.position.x > 48f)
        {
            Destroy(this.gameObject);
        }
    }

    void EnemyMoveDiagUp()
    {
        _enemyWeapon = true;
        transform.Translate(new Vector3(-1, 0.25f, 0) * _speed * Time.deltaTime, Space.World);
        if (transform.position.x < -37f)
        {
            Destroy(this.gameObject);
        }
    }

}
