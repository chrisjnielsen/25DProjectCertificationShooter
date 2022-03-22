using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    private static AudioPlayer _instance;
    public static AudioPlayer Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("AudioPlayer Instance is NULL");
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    private AudioSource _audio;
    private AudioSource _bgMusic;
    [SerializeField]
    private AudioClip[] _clips;
    [SerializeField]
    private AudioMixer _mixer;

    private void Awake()
    {
        _instance = this;
        _audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        /*Debug.Log("AudioVolume: "+_audio.volume);
        Debug.Log("PlayerPrefs Audio: " + PlayerPrefs.GetFloat("Volume"));
        if (PlayerPrefs.GetFloat("Volume") != 0)
        {
            _audio.volume = Mathf.Log10(PlayerPrefs.GetFloat("Volume")) * 20;
        }
        else _audio.volume = Mathf.Log10(0.4f) * 20;*/
        
    }

    public void playWeaponUpSound()
    {
        _audio.clip = _clips[1];
        _audio.Play();
    }

    public void playCoinSound()
    {
        _audio.clip = _clips[0];
        _audio.Play();

    }

    public void playShieldSound()
    {
        _audio.clip = _clips[2];
        _audio.Play();
    }

    public void playEnemyDamage()
    {
        _audio.clip = _clips[3];
        _audio.Play();
    }

    public void playDestroyed()
    {
        _audio.clip = _clips[4];
        _audio.Play();
    }

    public void playMissileBy()
    {
        _audio.clip = _clips[5];
        _audio.Play();
    }

}
