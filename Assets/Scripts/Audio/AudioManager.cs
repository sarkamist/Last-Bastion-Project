using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    #region Properties
    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource _musicSource;
    public AudioSource MusicSource
    {
        get => _musicSource;
        set => _musicSource = value;
    }

    [SerializeField]
    private AudioSource _sfxSource;
    public AudioSource SFXSource {
        get => _sfxSource;
        set => _sfxSource = value;
    }

    [Header("Audio Clips — Themes")]
    [SerializeField]
    private AudioClip _clip_MenuTheme;
    public AudioClip Clip_MenuTheme
    {
        get => _clip_MenuTheme;
        set => _clip_MenuTheme = value;
    }

    [SerializeField]
    private AudioClip _clip_FightTheme;
    public AudioClip Clip_FightTheme
    {
        get => _clip_FightTheme;
        set => _clip_FightTheme = value;
    }

    [SerializeField]
    private AudioClip _clip_DefeatTheme;
    public AudioClip Clip_DefeatTheme
    {
        get => _clip_DefeatTheme;
        set => _clip_DefeatTheme = value;
    }


    [Header("Audio Clips — SFX")]
    [SerializeField]
    private AudioClip _clip_OpenMenu;
    public AudioClip Clip_OpenMenu
    {
        get => _clip_OpenMenu;
        set => _clip_OpenMenu = value;
    }

    [SerializeField]
    private AudioClip _clip_CloseMenu;
    public AudioClip Clip_CloseMenu
    {
        get => _clip_CloseMenu;
        set => _clip_CloseMenu = value;
    }

    [SerializeField]
    private AudioClip _clip_NewWave;
    public AudioClip Clip_NewWave
    {
        get => _clip_NewWave;
        set => _clip_NewWave = value;
    }
    #endregion

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SFXSource.ignoreListenerVolume = true;
        MusicSource.ignoreListenerVolume = true;
    }

    private void Start()
    {
        PlayTheme(Clip_MenuTheme);
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void StopSFX()
    {
        SFXSource.clip = null;
        SFXSource.Stop();
    }

    public void PlayTheme(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    public void StopTheme() {
        MusicSource.clip = null;
        MusicSource.Stop();
    }
}
