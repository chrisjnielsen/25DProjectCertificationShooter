using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float _speed = 6f;
    [SerializeField]
    protected float yMax = 9f;
    [SerializeField]
    protected float yMin = -9f;
    [SerializeField]
    protected GameObject _enemyMissile;
    [SerializeField]
    protected GameObject _shieldPrefab;
    [SerializeField]
    protected List<GameObject> waypoints;
    [SerializeField]
    protected int wayPointIndex = 0;
    protected UIManager _uiManager;
    protected Player _player;
    protected Animator _anim;
    protected AudioClip _enemyExplosion;
    protected AudioSource _audioSource;
    [SerializeField]
    protected float _fireRate = 3f;
    [SerializeField]
    protected float _canFire = 3f;
    [SerializeField]
    protected bool _hasShield = false;
    [SerializeField]
    protected bool _followPlayer = false;
    [SerializeField]
    protected Vector3 _startPosition;
    [SerializeField]
    protected int _distToPowerup = 10;
    [SerializeField]
    protected Quaternion _startRotation;
    protected Quaternion _originalRotationValue;
    protected Rigidbody2D rb;
    protected float _rotateSpeed = 1f;
    protected WeaponFire enemyFire;
    protected MidBossFire enemyFire2;
    [SerializeField]
    protected bool enemyPowerUp;
    protected int _hitCount = 0;
    protected float _lowFireRate = 3f;
    protected float _highFireRate = 6f;
    protected int _diffFactor;
    protected bool allowFire, enemyDeathCalled;
    [SerializeField]
    protected GameObject _explosion1, _explosion2;
    [SerializeField]
    protected GameObject _enemyDestroyed;
    [SerializeField]
    protected GameObject _enemyFlashDamage;


    // Start is called before the first frame update
    public virtual void Start()
    {
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
        _enemyFlashDamage = transform.Find("Mirage2000").gameObject;
        _enemyFlashDamage.gameObject.SetActive(false);
    }

    public virtual void Update()
    {
        if (allowFire == true && _player != null && transform.position.y>-13 && transform.position.y <14f && enemyDeathCalled ==false)
        {
            StartCoroutine(FireRate());
        }

        if (_player != null)
        {
            //if (Vector3.Distance(_player.transform.position, transform.position) > 3f) _followPlayer = false;
            //if (Vector3.Distance(_player.transform.position, transform.position) <= 3f) _followPlayer = true;
            CalculateMovement();
        }
        CheckHitPowerUp();
        
    }

    IEnumerator FireRate()
    {
        allowFire = false;
        enemyFire.EnemyMissile();
        yield return new WaitForSeconds(Random.Range(_lowFireRate, _highFireRate));
        allowFire = true;
    }

    public virtual void CalculateMovement()
    {
        /*switch (_followPlayer)
        {
            case true:
                // Get a direction vector from us to the target
                Vector3 dir = _player.transform.position - transform.position;
                // Normalize it so that it's a unit direction vector
                dir.Normalize();
                RotateTowards(_player.transform.position);
                // Move ourselves in that direction
                transform.position += dir * _speed * Time.deltaTime;
                break;*/
        //case false:
        //restore default rotation and movement
        /*transform.rotation = Quaternion.Slerp(transform.rotation, _originalRotationValue, Time.deltaTime * _speed);
        transform.Translate(new Vector3(0, -1, 0) * _speed * Time.deltaTime);
        if (transform.position.x < -7f)
        {
            transform.position = new Vector3(49f, Random.Range(yMin, yMax), 0);
        }*/
        //break;
        //}
    }

    /*public virtual void Dodge(float value)
    {
        transform.position = new Vector2(transform.position.x + value, transform.position.y);
    }

    public virtual void RotateTowards(Vector2 target)
    {
        var offset = 90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }*/

    public virtual void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                Player player = other.GetComponent<Player>();
                this.GetComponent<MeshCollider>().enabled = false;
                player.Damage();
                StartCoroutine(EnemyDeath());
                break;
            case "Bullet":
                Destroy(other.gameObject);
                other.GetComponent<BoxCollider>().enabled = false;
                _hitCount++;
                AudioPlayer.Instance.playEnemyDamage();
                StartCoroutine(FlashDamage());
                _explosion1.gameObject.SetActive(true);
                if (_hitCount > 5) 
                {
                    StartCoroutine(EnemyDeath());
                }
                break;
            case "BulletPowered":
                Destroy(other.gameObject);
                other.GetComponent<BoxCollider>().enabled = false;
                _hitCount++;
                _hitCount++;
                AudioPlayer.Instance.playEnemyDamage();
                StartCoroutine(FlashDamage());
                _explosion1.gameObject.SetActive(true);
                if (_hitCount > 5)
                {
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

    public void PowerUpEnemy()
    {
        enemyPowerUp = true;
        InvokeRepeating("Flash", 0, 0.25f);
    }

    IEnumerator FlashDamage()
    {
        _enemyFlashDamage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        _enemyFlashDamage.gameObject.SetActive(false);
    }

    public void Flash()
    {
        if (_enemyFlashDamage.gameObject.activeSelf) _enemyFlashDamage.gameObject.SetActive(false);
        else _enemyFlashDamage.gameObject.SetActive(true);
    }

   void CheckHitPowerUp()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.left * _distToPowerup);
        
        if (Physics.Raycast(transform.position, Vector3.left, out hit, _distToPowerup))
        {
            if (hit.collider == null)
            {
                return;
            }
            else if (hit.collider.tag == "Upgrade")
            {
                if (Time.time > _canFire + _fireRate && _player != null && transform.position.y > -13 && transform.position.y < 14f)
                {
                    _fireRate = Random.Range(1f, 3f);
                    enemyFire.EnemyMissile();
                    _canFire = Time.time;
                }
            }
        }
    }

    IEnumerator EnemyDeath()
    {   // update number of enemies in wave, and update UI
        CancelInvoke("Flash");
        enemyDeathCalled = true;
        allowFire = false;
        AudioPlayer.Instance.playDestroyed();
        _enemyFlashDamage.gameObject.SetActive(false);
        GameObject go = Instantiate(_enemyDestroyed, this.transform.position, Quaternion.identity);
        GetComponent<MeshCollider>().enabled = false; // disable collider on death cycle so no more chance they will cause damage to Player
        GetComponent<MeshRenderer>().enabled = false;
        _explosion1.gameObject.SetActive(false);
        go.transform.parent = null;
        GameManager.Instance.CurrentEnemyCount--;
        _player.AddScore(Random.Range(50, 100));
        _uiManager.UpdateEnemyCount();  
        transform.Translate(Vector3.zero);
        Destroy(go, 0.6f);
        Destroy(this.gameObject, 1f);
        yield return null;
    }
}

