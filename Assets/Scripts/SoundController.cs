using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioClip _tapSound;
    [SerializeField] private AudioClip _captureSound;
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioClip _loseSound;

    [SerializeField] private AudioSource _audio;

    public static SoundController Instance = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
    }

    public void OnWin()
    {
        _audio.clip = _winSound;
        
        if (_audio.clip != null)
        {
            _audio.Play();
        }
    }

    public void OnLose()
    {
        _audio.clip = _loseSound;
        
        if (_audio.clip != null)
        {
            _audio.Play();
        }
    }

    public void OnTap()
    {
        _audio.clip = _tapSound;
        
        if (_audio.clip != null)
        {
            _audio.Play();
        }
    }

    public void OnCapture()
    {
        _audio.clip = _captureSound;
        
        if (_audio.clip != null)
        {
            _audio.Play();
        }
    }
}