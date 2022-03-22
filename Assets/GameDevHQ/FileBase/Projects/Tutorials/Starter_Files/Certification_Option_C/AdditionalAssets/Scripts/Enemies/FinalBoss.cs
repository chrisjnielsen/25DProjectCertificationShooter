using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : Enemy
{ 
    public int enemyHitCount;
    private FinalBossFire bossFire;

    int _switchWaypointsMidway, _switchWaypointsFinal;
    float _speedFactor;
    float _fireFactor;

    public override void Start()
    {
        
        enemyHitCount = 0;
        BossWaypoints1();
        bossFire = GetComponent<FinalBossFire>();
        _fireFactor = 1f;
        _speedFactor = 1f;
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

    void BossWaypoints1()
    {
        waypoints = GameManager.Instance.BossWaypoints;
        this.transform.position = waypoints[wayPointIndex].transform.position;
    }

    public override void Update()
    {
        if (Time.time > _canFire + _fireRate && _player != null && transform.position.y > -13 && transform.position.y < 14f && allowFire == true)
        {
            _fireRate = Random.Range(_lowFireRate*_fireFactor, _highFireRate*_fireFactor);
            bossFire.FinalBossFireWeapon();
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
        if (waypoints.Count == 0 && _switchWaypointsMidway == 0 && _switchWaypointsFinal == 0)
        {
            transform.Translate(Vector3.zero);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[wayPointIndex].transform.position, _speed * _speedFactor * Time.deltaTime);
            if (transform.position == waypoints[wayPointIndex].transform.position) wayPointIndex += 1;
            if (wayPointIndex == waypoints.Count) wayPointIndex = 0;
        }
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
                Destroy(other.gameObject);
                other.GetComponent<BoxCollider>().enabled = false;
                enemyHitCount++;
                StartCoroutine(FlashDamage());
                if (enemyHitCount > 20)
                {
                    _switchWaypointsMidway++;
                    if (_switchWaypointsMidway == 1) BossMidStageWaypoints();
                    _explosion1.gameObject.SetActive(true);
                }
                if (enemyHitCount > 40)
                {
                    _switchWaypointsFinal++;
                    if (_switchWaypointsFinal == 1) BossFinalStageWaypoints();
                    _explosion2.gameObject.SetActive(true);
                }
                if (enemyHitCount >= 60)
                {
                    _player.AddScore(Random.Range(150, 300));
                    StartCoroutine(EnemyDeath());
                }

                //}
                break;

            case "BulletPowered":
                Destroy(other.gameObject);
                other.GetComponent<BoxCollider>().enabled = false;
                enemyHitCount++;
                enemyHitCount++;
                StartCoroutine(FlashDamage());
                if (enemyHitCount > 20)
                {
                    _switchWaypointsMidway++;
                    if (_switchWaypointsMidway == 1) BossMidStageWaypoints();
                    _explosion1.gameObject.SetActive(true);
                }
                if (enemyHitCount > 40)
                {
                    _switchWaypointsFinal++;
                    if (_switchWaypointsFinal == 1) BossFinalStageWaypoints();
                    _explosion2.gameObject.SetActive(true);
                }
                if (enemyHitCount >= 60)
                {
                    _player.AddScore(Random.Range(150, 300));
                    StartCoroutine(EnemyDeath());
                }

                //}
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

    void BossMidStageWaypoints()
    {
        _speedFactor = 1.5f;
        _fireFactor = .75f;
        waypoints.Clear();
        wayPointIndex = 0;
        GameManager.Instance.finalBossWayPointsFound = 0;
        waypoints = GameManager.Instance.BossWaypoints2ndStage;
        this.transform.position = waypoints[wayPointIndex].transform.position;
    }

    void BossFinalStageWaypoints()
    {
        _speedFactor = 2f;
        _fireFactor = .25f;
        waypoints.Clear();
        wayPointIndex = 0;
        GameManager.Instance.finalBossWayPointsFound = 0;
        waypoints = GameManager.Instance.BossWaypointsFinalStage;
        this.transform.position = waypoints[wayPointIndex].transform.position;
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
        _uiManager.UpdateEnemyCount();
        transform.Translate(Vector3.zero);
        Destroy(go, 0.6f);
        Destroy(this.gameObject, 1.6f);
        yield return null;
    }
}
