using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private PlayerActions playerInput;
    [SerializeField]
    private float _speed, _horizontalInput, _verticalInput;
    [SerializeField]
    private LayerMask _layer;
    private PlayerAnimation _anim;
    
    private WeaponFire _weapon;
    [SerializeField]
    private int weaponIndex;
    private UIManager _uiManager;
    private AudioSource _audio;
    [SerializeField]
    private AudioClip[] _clipArray = new AudioClip[3];

    public int Health { get; set; }

    private AudioSource _audioSource;
    public int coins;
    [SerializeField]
    private GameObject _thruster;
    private float _speedupFactor = 1.0f;
    private Animator _animator;
    public int _playerHitCount = 5;
    public bool _playerHasShield = false;
    private float firingTimer, fireDelay;
    private Quaternion _origRotate, _forwardRotate, _backRotate;
    private bool _check;


    private void Awake()
    {
        playerInput = new PlayerActions();
        _controller = GetComponent<CharacterController>();
        _weapon = transform.GetComponentInChildren<WeaponFire>();
        weaponIndex = 0;
        _uiManager=GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        _audio = GetComponent<AudioSource>();
        if (_audio == null)
        {
            Debug.LogError("Audio Source on Player is NULL");
        }
        _thruster = transform.GetChild(5).gameObject;
        _animator = GetComponentInChildren<Animator>();
        _animator.SetFloat("Blend", 0f);
        fireDelay = .1f;
        firingTimer = fireDelay;
        if (GameObject.FindGameObjectWithTag("Checkpoint") != null)
        {
            _check = GameObject.FindGameObjectWithTag("Checkpoint").GetComponent<CheckpointMaster>().checkpoint;
        }

    }


    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Start()
    {
        _controller.radius = 0;
        _controller.height = 0;
        _origRotate = gameObject.transform.rotation;
        _forwardRotate = Quaternion.Euler(0, 0, -8f);
        _backRotate = Quaternion.Euler(0, 0, 2f);
        if (_check == true)
        {
            coins = PlayerPrefs.GetInt("playerscore");
        }
    }

    void Update()
    {
        Movement();
        if (_horizontalInput != 0 || _verticalInput != 0) _animator.SetBool("Rotors", true);

        if (firingTimer > 0)
        {
            firingTimer -= Time.deltaTime;
        }

        if (playerInput.Keyboard.RegularWeapon.triggered && weaponIndex <4)
        {
            if (firingTimer < 0)
            {
                
                _weapon.FireWeapon(weaponIndex);
                PlayAudio();
                firingTimer += fireDelay;
            }
                        
        }

        if (playerInput.Keyboard.SpecialWeapon.IsPressed() == true && weaponIndex ==4)
        {
            _weapon.FireWeapon(weaponIndex);
            PlayAudio();
        }

        
        if (playerInput.Keyboard.Speed.IsPressed() == true)
        {
            SpeedUp();
        }
        else SpeedOff();
    }

    void SpeedUp()
    {
        _thruster.gameObject.SetActive(true);
        _speedupFactor = 1.5f;
    }

    void SpeedOff()
    {
        _thruster.gameObject.SetActive(false);
        _speedupFactor = 1.0f;
    }

    void Movement()
    {
        Vector2 movementInput = playerInput.Keyboard.Movement.ReadValue<Vector2>();
        _horizontalInput = movementInput.x;
        if (_horizontalInput > 0) TiltForward();
        if (_horizontalInput < 0) TiltBack();
        if (_horizontalInput == 0) TiltNeutral();
        _verticalInput = movementInput.y;
        _controller.Move(movementInput * _speed *_speedupFactor* Time.deltaTime);
    }

    void TiltForward()
    {
        transform.rotation = Quaternion.RotateTowards(_origRotate,_forwardRotate, _speed * Time.deltaTime);
        StartCoroutine(PauseBetweenMovement());
        transform.rotation = Quaternion.RotateTowards(_forwardRotate, _origRotate, _speed * Time.deltaTime);
    }

    void TiltNeutral()
    {
        transform.rotation = _origRotate;
    }

    void TiltBack()
    {
        transform.rotation = Quaternion.RotateTowards(_origRotate, _backRotate, _speed * Time.deltaTime);
        StartCoroutine(PauseBetweenMovement());
        transform.rotation = Quaternion.RotateTowards(_backRotate, _origRotate, _speed * Time.deltaTime);
    }

    IEnumerator PauseBetweenMovement()
    {
        yield return new WaitForSeconds(1f);
    }

    public void WeaponUp()
    {
        weaponIndex++;
        if (weaponIndex > 3) weaponIndex = 3;
        _uiManager.UpdateWeaponLevel(weaponIndex);
    }

    public void WeaponDown()
    {
        if (weaponIndex > 0)
        {
            weaponIndex--;
            _uiManager.UpdateWeaponLevel(weaponIndex);
        }
        else PlayerDeath();
  
    }

    private void PlayAudio()
    {
        _audio.clip = _clipArray[weaponIndex];
        _audio.Play();
    }

    public void PlayerShield()
    {
        _animator.SetFloat("Blend", 1.0f);
        _playerHitCount = 5;
        _playerHasShield = true;
        _uiManager.UpdateShield(_playerHitCount);
    }

    void PlayerDeath()
    {
        AudioPlayer.Instance.playDestroyed();
        SpawnScriptObj.Instance.OnPlayerDeath();
        Destroy(this.gameObject, 1f);
        _uiManager.GameOverSequence();
        GameManager.Instance.GameOver();
        // _anim.Death();
    }

    public void Damage()
    {
        if (_playerHasShield == true)
        {
            if (_playerHitCount > 0)
            {
                _playerHitCount--;
                _uiManager.UpdateShield(_playerHitCount);
            }
            else
            {
                _playerHasShield = false;
                _playerHitCount = 0;
                _uiManager.UpdateShield(_playerHitCount);
                _animator.SetFloat("Blend", 0f);
            }
        }
        else
        {
            WeaponDown();
        }        
    }

    public void AddScore(int amount)
    {
        coins += amount;
        _uiManager.UpdateScore(coins);
    }
}
