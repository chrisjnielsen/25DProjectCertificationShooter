using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    public int enemyHitCount;

    public override void Start()
    {
        
        enemyHitCount = 0;
        waypoints = GameManager.Instance.Waypoints2;
        this.transform.position = waypoints[wayPointIndex].transform.position;
        SpawnScriptObj.Instance.Checkpoint();
        _diffFactor = PlayerPrefs.GetInt("Difficulty");
        switch (_diffFactor)
        {
            case (0):
                _lowFireRate = 3f;
                _highFireRate = 6f;
                _speed = 6f;
                break;
            case (1):
                _lowFireRate = 2f;
                _highFireRate = 5f;
                _speed = 12f;
                break;
            case (2):
                _lowFireRate = .75f;
                _highFireRate = 1.5f;
                _speed = 20f;
                break;
            default:
                break;
        }
        _originalRotationValue = transform.rotation;
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        else return;

        _anim = GetComponent<Animator>();

        _uiManager = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("UI Manager is NULL");
        }
        enemyFire = transform.GetComponentInChildren<WeaponFire>();
        enemyFire2 = transform.GetComponentInChildren<MidBossFire>();
        allowFire = true;
        enemyDeathCalled = false;

    }
    public override void Update()
    {
        if (Time.time > _canFire + _fireRate && _player != null && transform.position.y > -13 && transform.position.y < 14f)
        {
            _fireRate = Random.Range(_lowFireRate, _highFireRate);
            enemyFire.EnemyBullet();
            enemyFire2.MidBossFireWeapon();
            _canFire = Time.time;
        }

        if (_player != null)
        {
            CalculateMovement();
        }
    }
    public override void CalculateMovement()
    {
        //enemy follows set of waypoints instead of basic movement
        transform.position = Vector3.MoveTowards(transform.position, waypoints[wayPointIndex].transform.position, _speed *  Time.deltaTime);
        if (transform.position == waypoints[wayPointIndex].transform.position) wayPointIndex += 1;
        if (wayPointIndex == waypoints.Count) wayPointIndex = 0;
    }

    public override void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                if (_hasShield == true)
                {
                    _player.Damage();
                    _hasShield = false;
                }
                else if (_hasShield == false)
                {
                    GetComponent<MeshCollider>().enabled = false;
                    _player.Damage();
                    StartCoroutine(EnemyDeath());
                }
                break;
            case "Bullet":
                /*if (_hasShield == true)
                {
                    Destroy(other.gameObject);
                    _hasShield = false;
                }
                else if (_hasShield == false)
                {*/
                enemyHitCount++;
                AudioPlayer.Instance.playEnemyDamage();
                StartCoroutine(FlashDamage());
                Destroy(other.gameObject);
                other.GetComponent<BoxCollider>().enabled = false;
                if (enemyHitCount > 5) _explosion1.gameObject.SetActive(true);
                if (enemyHitCount > 20) _explosion2.gameObject.SetActive(true);
                if (enemyHitCount >= 30)
                {
                    _player.AddScore(Random.Range(50, 100));
                    StartCoroutine(EnemyDeath());
                }
                
                
                //}
                break;
            case "BulletPowered":
                Destroy(other.gameObject);
                other.GetComponent<BoxCollider>().enabled = false;
                enemyHitCount++;
                enemyHitCount++;
                AudioPlayer.Instance.playEnemyDamage();
                StartCoroutine(FlashDamage());
                if (enemyHitCount > 5) _explosion1.gameObject.SetActive(true);
                if (enemyHitCount > 20) _explosion2.gameObject.SetActive(true);
                if (enemyHitCount >= 30)
                {
                    _player.AddScore(Random.Range(50, 100));
                    StartCoroutine(EnemyDeath());
                }
                break;
            case "Shield":
                if (_hasShield == true)
                {
                    _hasShield = false;
                    return;
                }
                else if (_hasShield == false)
                {
                    _player.Damage();
                    _player.AddScore(Random.Range(5, 11));
                    StartCoroutine(EnemyDeath());
                }
                break;
            case "Enemy":
                return;

            case "Enemy2":
                return;

            case "HomingMissile":
                {
                    if (_hasShield == true)
                    {
                        Destroy(other.gameObject);
                        _hasShield = false;
                    }
                    else if (_hasShield == false)
                    {
                        Destroy(other.gameObject);
                        //    _player.AddScore(Random.Range(10, 20));
                        //StartCoroutine(EnemyDeath());
                    }
                    break;
                }

            default:
                break;
        }
    }

    IEnumerator FlashDamage()
    {
        _enemyFlashDamage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _enemyFlashDamage.gameObject.SetActive(false);
    }

    IEnumerator EnemyDeath()
    {   // update number of enemies in wave, and update UI

        allowFire = false;
        AudioPlayer.Instance.playDestroyed();
        GameObject go = Instantiate(_enemyDestroyed, this.transform.position, Quaternion.identity);
        GetComponent<MeshCollider>().enabled = false; // disable collider on death cycle so no more chance they will cause damage to Player
        GetComponent<MeshRenderer>().enabled = false;
        _explosion1.gameObject.SetActive(false);
        _explosion2.gameObject.SetActive(false);
        go.transform.parent = null;
        GameManager.Instance.CurrentEnemyCount--;
        _player.AddScore(Random.Range(50, 100));
        _uiManager.UpdateEnemyCount();
        transform.Translate(Vector3.zero);
        GameManager.Instance.MidBossBeaten();
            //Animator trigger
            //_anim.SetTrigger("OnEnemyDeath");

            //_canFire = -1;
            //_audioSource.Play();
        Destroy(go, 0.6f);
        Destroy(this.gameObject, 2f);
        yield return null;
    }
}
